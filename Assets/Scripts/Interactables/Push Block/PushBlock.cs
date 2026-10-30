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
    [SerializeField] private float timeToMove = 5f;

    //Non-Inspector Variables
    private bool isMoving = false;
    private bool interactable = true;
    private Transform startPoint;
    private Transform endPoint;

    private float timeElapsed = 0f;

    [Header("Boxcast Vars")]
    private Vector3 boxSize = Vector3.one;
    private float MaxCheckDistance = 4f; 
    [SerializeField] private LayerMask TargetLayer;
    [SerializeField] private LayerMask BlockLayer;
    [Header("Variants")]
    [SerializeField] private bool iceBlock = false;
    private int lastDirectionPushed = 0;

    // DrawGizmos Variables
    [SerializeField] private bool drawBoxCasts = true;

    // Stored Positions for move
    private GameObject northHit = null;
    private GameObject southHit = null;
    private GameObject eastHit = null;
    private GameObject westHit = null;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        PostMoveReset();
    }

    // Update is called once per frame
    void Update()
    {
        InteractionCheck();
        HandleMovement();
    }
    #endregion

    #region Movement and Interaction Methods

    // can still be used for moving the block but we change the positions of startPoint and endPoint
    private void HandleMovement()
    {
        // Check if the block is allowed to move
        if (isMoving && startPoint != null && endPoint != null)
        {
            // prevent extra interactions
            interactable = false;

            // Calculate the time elapsed and the interpolation factor
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / timeToMove;

            // Lerp block from set start point to set end point
           
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
    }

    public void MoveCheck()
    {
        // Boxcast in the 4 cardinal directions, if we detech an object that notates that the block can move to that location, set the corresponding variable to true. 
        // For interactions, check where the player is relative to the block. If they are +x +y move it accordingly, should the block be allowed to move in that direction.
        // Set a boolean for blocks with continuous/ice movement. Those blocks, once interacted with, will continue to move until they encounter an area they cannot move past (no indicator object)
        // Invisible tagged objects will handle the grid system, and I will use coloured planes to indicate their locations for now.
        // MAKE SURE that these objects CANNOT BE SEEN FROM OTHER GRIDS bc then we could get boxes moving way off track from where they should be.
        // For ice mode, make the object check at the end of the LERP but ONLY in the direction it was moving, and check if an object occupies that spot already (blocks can block blocks)
        // When detected, get the object's position and allow movement directly to that position via LERP
    }

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
                return;
            }

        }

        Debug.Log("Ice Slide Succeeded");

    }

    public override void DolphinInteract()
    {
        base.DolphinInteract();

        if (interactableByDolphin && interactable)
        {
            // Get the difference in positions between objects
            if (PlayerRef == null) return;
            Vector3 difference = PlayerRef.transform.position - transform.position;

            // Compare the distances along the X and Z axises
            if (Mathf.Abs(difference.z) > Mathf.Abs(difference.x))
            {
                // Player is closer to the North or South side.
                if (difference.z < 0)
                {
                    // North push logic
                    Debug.Log("Pushing North");
                    if (northHit != null)
                    {
                        endPoint = northHit.transform;
                        lastDirectionPushed = 1;
                        isMoving = true;
                    }
                    else
                    {
                        Debug.Log("No valid spot to push to");
                    }
                }
                else
                {
                    // South push logic
                    Debug.Log("Pushing South");
                    if (southHit != null)
                    {
                        endPoint = southHit.transform;
                        lastDirectionPushed = 2;
                        isMoving = true;
                    }
                    else
                    {
                        Debug.Log("No valid spot to push to");
                    }
                }
            }
            else
            {
                // Player is closer to the East or West side.
                if (difference.x < 0)
                {
                    // East push logic
                    Debug.Log("Pushing East");
                    if (eastHit != null)
                    {
                        endPoint = eastHit.transform;
                        lastDirectionPushed = 3;
                        isMoving = true;
                    }
                    else
                    {
                        Debug.Log("No valid spot to push to");
                    }
                }
                else
                {
                    // West push logic
                    Debug.Log("Pushing West");
                    if (westHit != null)
                    {
                        endPoint = westHit.transform;
                        lastDirectionPushed = 4;
                        isMoving = true;
                    }
                    else
                    {
                        Debug.Log("No valid spot to push to");
                    }
                }
            }
        }
    }

    private void PostMoveReset()
    {
        isMoving = false;
        timeElapsed = 0f;

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
        boxCastChecker(Vector3.forward, ref northHit); // North Check
        boxCastChecker(Vector3.right, ref eastHit); // East Check
        boxCastChecker(Vector3.back, ref southHit); // South Check
        boxCastChecker(Vector3.left, ref westHit); // West Check
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

        // FOR NOW the only thing we are checking for is other moving blocks. Adding other raycasts that detect things in the way will be easy as copying the above code for obstructed and switching the target layer, but since we haven't set up normal wall obstacles or anything yet between the grid, I'll leave it to take care of at a later date

        if ( obstructed ) { 
            canMove = false; 
        }

        hitLocation = canMove ? hit.collider.gameObject : null;

        if (drawBoxCasts)
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying) { 
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

#endregion

#region DrawGizmos

private void OnDrawGizmos()
    {
        boxCastChecker(Vector3.forward, ref northHit); // North Check
        boxCastChecker(Vector3.right, ref eastHit); // East Check
        boxCastChecker(Vector3.back, ref southHit); // South Check
        boxCastChecker(Vector3.left, ref westHit); // West Check
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
