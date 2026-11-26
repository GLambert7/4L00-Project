using System;
using System.Drawing;
using UnityEngine;

public class PushBlock : BaseInteractable
{
    /// <summary>
    /// How the script should function:
    /// - The block exists on a grid system, and when pushed moves as far as possible along the grid until it hits an obstacle or reaches the end of the grid.
    /// - The block is always moved by the Dolphin
    /// - Moves based on look direction
    /// - 
    /// </summary>
    
    // Attach this script to the block you wish to move. Create two empty game objects and place them at the two points you want the block to move between, then assign these two empty game objects to the
    // startPoint and endPoint variables in the inspector. Add an additional collider to the block to define the interaction area, and ensure it is set to "Is Trigger". The block will move between the two
    // set points when the player interacts with it. Interacting with the block will cause it to move from its current position to the other point. The block will only start moving when the player is in range
    // and presses the interact button. The player should be tagged as "Player" to ensure it is detected correctly. More logic can be added onto this script in the future depending on the needs of the puzzle
    // designers, such as adding more points for the block to move between, conditions for the block to move, or adding a grid system for the block to move on. I made it simple and easy for now tho.
    
    /*
     * TODO:
     * Add more points for the block to move between (currently only supports two points)
     * Add conditions that allow or prevent the block from to moving
     * 
     * CURRENT BUGS:
     * Cube can kind of clip through player during movement, need to make sure the player is pushed out of the way when the block moves. Currently I am handling this via a rigidbody on the block, but this is not a 
     * perfect solution, as it seems to pop the player up instead of pushing them out of the way or smoothly moving them with the block. I will need to look into this more and find a better solution.
     * 
     * If there's anything else you want me to add, please let me know and I will do my best to implement it. 
     */

    #region Variables

    //Inspector Variables
    [Header("Push Block Specifics")]
    [Tooltip("How long it takes for the block to move from one point to another")]
    [SerializeField] private float timeToMove = 1f;

    [Header("Falling Block Specifics")]
    [Tooltip("Can the block fall?")]
    [SerializeField] private bool canFall = true;
    [Tooltip("How fast the block falls")]
    [SerializeField] private float fallSpeed = 5f;

    [Header("Boxcast Vars")]
    [Tooltip("What layer is the block looking for to move to?")]
    [SerializeField] private LayerMask TargetLayer;
    [Tooltip("What layer is the block checking for that will inhibit its movement?")]
    [SerializeField] private LayerMask BlockLayer;
    [Tooltip("How big is the boxcast?")]
    [SerializeField] private Vector3 boxSize = Vector3.one;
    [Tooltip("How far are we checking for the next move location?")]
    [SerializeField]private float MaxCheckDistance = 4f;
    [Tooltip("How far to check below to stop falling?")]
    [SerializeField] private float fallCheckDepth = 0.2f;

    [Header("Block Type Variants")]
    [Tooltip("Is the block an ice block? If so, it will slide until it hits an obstacle.")]
    [SerializeField] private bool iceBlock = false;
    [Tooltip("If the block is an ice block, should it rebound back to its original position after hitting an obstacle? <b>DO NOT USE WITHOUT ICE TOGGLED ON, IT WILL NOT WORK</b>")]
    [SerializeField] private bool rebound = false;
    [SerializeField] private bool bigBlock = false;



    // DrawGizmos Variables
    [Header("Gizmos")]
    [Tooltip("Should the boxcasts be drawn in the scene view?")]
    [SerializeField] private bool drawBoxCasts = true;




    //Non-Inspector Variables
    // "What do all of these variables do?" "Don't worry about it kitten." "Okay, yay!"

    private bool isMoving = false;
    public bool IsMoving => isMoving;
    private bool interactable = true;
    private Transform startPoint;
    private Transform endPoint;

    // Change to ENUM
    private int lastDirectionPushed = 0;
    
    private float timeElapsed = 0f;

    // Stored Positions for move
    public GameObject northHit = null;
    public GameObject southHit = null;
    public GameObject eastHit = null;
    public GameObject westHit = null;

    private bool reboundActive = false;
    private bool reboundInProgress = false;

    private bool beingLifted = false;

    private bool floorDetected = false;

    private float timeBetweenChecks = .15f;
    private float checksTimer = 0f;
    private bool inCurrent = false;
    public bool inCurrentMovement = false;

    public bool currentBreaker = false;

    // Called by the plant to control if the block is being lifted
    public void SetBeingLifted(bool lifted)
    {
        beingLifted = lifted;
    }

    //saves the position fo the block
    private Vector3 savedPushPosition;
    private bool hasSavedPushPosition = false;

    private CompanionLimiter limiter;

    [SerializeField] private bool displayDuringRun = true;




    #endregion

    #region Unity Methods

    public override void Awake()
    {
        base.Awake();
        if (limiter == null && bigBlock)
        {
            limiter = GetComponent<CompanionLimiter>();
        }

        PostMoveReset();
    }

    // Update is called once per frame
    void Update()
    {
        InteractionCheck();

        // Don't apply falling while the plant is lifting the block.
        if (canFall && !beingLifted)
        {
            FallCheck();
        }

        HandleMovement();
    }
    #endregion

    #region Movement and Interaction Methods

    // can still be used for moving the block but we change the positions of startPoint and endPoint
    private void HandleMovement()
    {
        // Check if the block is allowed to move
        if (!isMoving || startPoint == null || endPoint == null)
        {
            inCurrentMovement = false;
            reboundInProgress = false;
            return;
        }

        // prevent extra interactions
        interactable = false;
        canFall = false;

        // Calculate the time elapsed and the interpolation factor
        timeElapsed += Time.deltaTime;
        float t = timeElapsed / timeToMove;

        // Lerp block from set start point to set end point DISABLE WHEN WE'RE IN A CURRENT
           
        transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);

        if (transform.position == endPoint.position) t = 1f;
            
        // If the interpolation factor is greater than or equal to 1, the block has reached its destination, so we stop moving and reset the time elapsed
        if (t >= 1f)
        {
            PostMoveReset();
            if (iceBlock)
            {
                iceSlide();
            }
        }        
    }

    private void FallCheck()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        //Grab our position
        Vector3 origin = box.bounds.center;
        origin.y = box.bounds.min.y - (fallCheckDepth/2f);

        //halfExtents is half the size of the box bc that's what the method checks for
        Vector3 halfExtents = new Vector3(box.bounds.extents.x * 0.9f, fallCheckDepth / 2f, box.bounds.extents.z * 0.9f);

        //Checks the direction, always straight down
        Vector3 direction = Vector3.down;

        // Use the same rotation for both the cast and the Gizmos.
        Quaternion boxRotation = transform.rotation;
        boxRotation.y = Quaternion.identity.y;

        LayerMask fallMask = ~LayerMask.GetMask("MoveTargets", "Player", "IgnoreForFalling", "GroWeedInactive");

        // Do the BoxCast with our calc'd info
        Collider[] hits = Physics.OverlapBox(
            origin, // position the cast starts from
            halfExtents, // how big the cast area is (half of the size of our box)
            boxRotation, // In case the box is rotated this will correct it
            fallMask // Exclusive layermask so we don't hit the moveTarget objects or the player
        );

        floorDetected = false;

        foreach (Collider hit in hits) 
        {
            if (hit.transform == transform || hit.transform.IsChildOf(transform)) continue;

            Debug.Log("Floor found from object: "+hit.gameObject.name);
            floorDetected = true;
            break;
        }


        if (!floorDetected) {
            transform.Translate( Vector3.down * fallSpeed * Time.deltaTime);
        }
    }

    public void FallCheckDrawGizmos()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        //Grab our position
        Vector3 origin = box.bounds.center;
        origin.y = box.bounds.min.y - 1f;

        //halfExtents is half the size of the box bc that's what the method checks for
        Vector3 halfExtents = new Vector3(box.bounds.extents.x * 0.9f, 0.05f, box.bounds.extents.z * 0.9f);

        //Checks the direction, always straight down
        Vector3 direction = Vector3.down;

        // Use the same rotation for both the cast and the Gizmos.
        Quaternion boxRotation = transform.rotation;

        LayerMask fallMask = ~LayerMask.GetMask("MoveTargets", "Player");

        // Do the BoxCast with our calc'd info
        bool floorDetected = Physics.CheckBox(
            origin, // position the cast starts from
            halfExtents, // how big the cast area is (half of the size of our box)
            boxRotation, // In case the box is rotated this will correct it
            fallMask // Exclusive layermask so we don't hit the moveTarget objects or the player
        );

        if (drawBoxCasts)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying || displayDuringRun)
            {
                // Red if the ground is detected, green if it isn't
                Gizmos.color = floorDetected ? UnityEngine.Color.red : UnityEngine.Color.green;

                // Draw the detection box
                DrawBox(
                    origin,
                    halfExtents,
                    boxRotation
                );
            }
#endif
        }

    }

    // One day I will optimize this code, but today is not that day. I will refactor this code in the future to make it cleaner and more efficient. ~Emilyyyyyyyyyy
    public void iceSlide()
    {

        if (lastDirectionPushed == 1) // North
        {
            boxCastChecker(Vector3.forward, ref northHit); // North Check
            if (northHit != null)
            {
                endPoint = northHit.transform;
                isMoving = true;
            }
            else 
            {
                Debug.Log("Ice Slide stopped");
                reboundActive = true;
                FallCheck();
                if (!floorDetected)
                {
                    PostMoveReset();
                }
                else if (rebound && reboundActive && !reboundInProgress)
                {
                    Rebound();
                }
                else if (bigBlock)
                {
                    limiter.DolphinInteract();
                }
                    return; 
            }
        }
        else if (lastDirectionPushed == 2) // South
        {
            boxCastChecker(Vector3.back, ref southHit); // South Check
            if (southHit != null)
            {
                endPoint = southHit.transform;
                isMoving = true;
            }
            else
            {
                Debug.Log("Ice Slide stopped");
                reboundActive = true;

                FallCheck();
                if (!floorDetected)
                {
                    PostMoveReset();
                }
                else if (rebound && reboundActive && !reboundInProgress)
                {
                    Rebound();
                }
                else if (bigBlock)
                {
                    limiter.DolphinInteract();
                }
                    return;
            }

        }
        else if (lastDirectionPushed == 3) // East
        {
            boxCastChecker(Vector3.right, ref eastHit); // East Check
            if (eastHit != null)
            {
                endPoint = eastHit.transform;
                isMoving = true;
            }
            else
            {
                Debug.Log("Ice Slide stopped");
                reboundActive = true;
                FallCheck();
                if (!floorDetected)
                {
                    PostMoveReset();
                }
                else if (rebound && reboundActive && !reboundInProgress)
                {
                    Rebound();
                }
                else if (bigBlock)
                {
                    limiter.DolphinInteract();
                }
                    return;
            }

        }
        else if (lastDirectionPushed == 4) // West
        {
            boxCastChecker(Vector3.left, ref westHit); // West Check
            if (westHit != null)
            {
                endPoint = westHit.transform;
                isMoving = true;
            }
            else
            {
                Debug.Log("Ice Slide stopped");
                reboundActive = true;
                FallCheck();
                if (!floorDetected)
                {
                    PostMoveReset();
                }
                else if (rebound && reboundActive && !reboundInProgress)
                {
                    Rebound();
                }else if (bigBlock)
                {
                    limiter.DolphinInteract();
                }
                    return;
            }

        }

        Debug.Log("Ice Slide Succeeded");

    }

    public void Rebound()
    {
        if (!reboundActive) return;
        reboundActive = false;
        reboundInProgress = true;
        if (lastDirectionPushed == 1) // North
        {
            boxCastChecker(Vector3.back, ref southHit); // South Check
            endPoint = southHit.transform;
            lastDirectionPushed = 2;
            isMoving = true;
        }
        else if (lastDirectionPushed == 2) // South
        {
            boxCastChecker(Vector3.forward, ref northHit); // North Check
            endPoint = northHit.transform;
            lastDirectionPushed = 1;
            isMoving = true;
        }
        else if (lastDirectionPushed == 3) // East
        {
            boxCastChecker(Vector3.right, ref eastHit); // East Check
            endPoint = westHit.transform;
            lastDirectionPushed = 4;
            isMoving = true;
        }
        else if (lastDirectionPushed == 4) // West
        {
            boxCastChecker(Vector3.left, ref westHit); // West Check
            endPoint = eastHit.transform;
            lastDirectionPushed = 3;
            isMoving = true;
        }
    }

    //I finally got the interaction stuff working but the blocks are moving based off where the player is and not where they were (this is reminding me of that missle meme lol) but this is bound to fix it
    public void SavePushPosition()
    {
        if (PlayerRef == null) return;

        savedPushPosition = PlayerRef.transform.position;
        hasSavedPushPosition = true;
    }

    // You know how I said "Today is not that day"? I LIED
    public void PushHandling(GameObject hit, int lastDir)
    {
        if (hit != null)
        {
            endPoint = hit.transform;
            lastDirectionPushed = lastDir;
            isMoving = true;
        }
        else
        {
            inCurrentMovement = false;
            Debug.Log("No valid spot to push to");
        }
    }

    public void currentBreak()
    {
        PostMoveReset();
        currentBreaker = false;
    }

    // Honestly all this directional shit can be optimized but I gotta make sure everything's good on the front end before I start optimizing and refactoring. ~Emilyyyyyyyyyy
    public override void DolphinInteract()
    {
        base.DolphinInteract();

        if (interactableByDolphin && interactable)
        {
            // Get the difference in positions between objects
            if (PlayerRef == null) return;
            //more position stuff for the move blocks
           Vector3 difference = (hasSavedPushPosition ? savedPushPosition : PlayerRef.transform.position) - transform.position;
            hasSavedPushPosition = false;

            // Compare the distances along the X and Z axises
            if (Mathf.Abs(difference.z) > Mathf.Abs(difference.x))
            {
                // Player is closer to the North or South side.
                if (difference.z < 0)
                {
                    // North push logic
                    Debug.Log("Pushing North");
                    PushHandling(northHit, 1);
                }
                else
                {
                    // South push logic
                    Debug.Log("Pushing South"); 
                    PushHandling(southHit, 2);
                }
            }
            else
            {
                // Player is closer to the East or West side.
                if (difference.x < 0)
                {
                    // East push logic
                    Debug.Log("Pushing East");
                    PushHandling(eastHit, 3);
                }
                else
                {
                    // West push logic
                    Debug.Log("Pushing West");
                    PushHandling(westHit, 4);
                }
            }
        }
    }

    private void PostMoveReset()
    {
        isMoving = false;
        timeElapsed = 0f;
        canFall = true;

        northHit = null;
        eastHit = null;
        southHit = null;
        westHit = null;

        endPoint = null;

        startPoint = this.transform;
        interactable = true;
        Debug.Log("Values Reset");
    }


    #endregion

    #region BoxCasts
    // Handle Boxcasts in FixedUpdate
    private void FixedUpdate()
    {
        checksTimer += Time.fixedDeltaTime;
        if (checksTimer >= timeBetweenChecks) {
            boxCastChecker(Vector3.forward, ref northHit); // North Check
            boxCastChecker(Vector3.right, ref eastHit); // East Check
            boxCastChecker(Vector3.back, ref southHit); // South Check
            boxCastChecker(Vector3.left, ref westHit); // West Check
            checksTimer = 0f;
        }
    }


    private void boxCastChecker(Vector3 localDirection, ref GameObject hitLocation)
    {
        //Grab our position
        Vector3 origin = transform.position;

        //halfExtents is half the size of the box bc that's what the method checks for
        Vector3 halfExtents = boxSize * 0.5f;

        // Convert the local direction into world space.
        Vector3 direction = transform.TransformDirection(localDirection).normalized;

        // Use the same rotation for both the cast and the Gizmos.
        Quaternion boxRotation = transform.rotation;

        // Do the BoxCast with our calc'd info
        bool canMove = Physics.BoxCast(
            origin, // position the cast starts from
            halfExtents, // how big the cast area is (half of the size of our box)
            direction, // Which way we're casting the box
            out RaycastHit hit, // The Raycast
            boxRotation, // In case the box is rotated this will correct it
            MaxCheckDistance, // How far is being checked
            TargetLayer // What layer are we looking for?
        );

        bool obstructed = Physics.BoxCast(
            origin, // position the cast starts from
            halfExtents, // how big the cast area is (half of the size of our box)
            direction, // Which way we're casting the box
            out RaycastHit hit2, // The Raycast
            boxRotation, // In case the box is rotated this will correct it
            MaxCheckDistance, // How far is being checked
            BlockLayer // What layer are we looking for?

        );

        // Old comment was outdated, just add the other layers in inspector to the BlockLayer and they'll be checked for obstruction. ~Emilyyyyyyyyyy

        if ( obstructed ) { 
            canMove = false; 
        }

        hitLocation = canMove ? hit.collider.gameObject : null;

        if (drawBoxCasts)
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying || displayDuringRun) { 
                Gizmos.color = canMove ? UnityEngine.Color.red : UnityEngine.Color.green;

                // Draw Start Box
                DrawBox(origin, boxSize, boxRotation);

                // Draw the end box, plus the line to it from start pos
                float distance = canMove ? hit.distance : MaxCheckDistance;
                Vector3 endPointCalc = origin + direction * distance;

                Gizmos.DrawLine(origin, endPointCalc);
                DrawBox(endPointCalc, boxSize, boxRotation );
            }
            #endif

        }


    }


    public void currentChange()
    {
        inCurrent = !inCurrent;
    }

    #endregion

    #region DrawGizmos
    // Shapes and colours the likes of which I've never seen before! ~Emilyyyyyyyyyy
    private void OnDrawGizmos()
    {
        boxCastChecker(Vector3.forward, ref northHit); // North Check
        boxCastChecker(Vector3.right, ref eastHit); // East Check
        boxCastChecker(Vector3.back, ref southHit); // South Check
        boxCastChecker(Vector3.left, ref westHit); // West Check
        FallCheckDrawGizmos();
    }

    private void DrawBox(Vector3 boxCenter, Vector3 boxSize, Quaternion boxRotation)
    {
        Matrix4x4 originalMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, boxRotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
        Gizmos.matrix = originalMatrix;
    }

    #endregion

}
