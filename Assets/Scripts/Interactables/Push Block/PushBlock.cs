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
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;
    [SerializeField] private float timeToMove = 5f;

    //Non-Inspector Variables
    private bool isMoving = false;
    private bool atPointA = true;
    private float timeElapsed = 0f;

    //Movecheck Variables
    private bool canMoveNorth = false;
    private bool canMoveSouth = false;
    private bool canMoveEast = false;
    private bool canMoveWest = false;

    // DrawGizmos Variables
    

    #endregion

    #region Unity Methods
    // Update is called once per frame
    void Update()
    {
        InteractionCheck();
        HandleMovement();
    }
    #endregion

    #region Movement and Interaction Methods
    private void HandleMovement()
    {
        // Check if the block is allowed to move
        if (isMoving)
        {
            // Calculate the time elapsed and the interpolation factor
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / timeToMove;

            // Check the block's current position (for now this is just A or B, but we can add more points later if we want to, change it to a grid system, the works) then lerp between the two points based on the interpolation factor
            if (atPointA)
            {
                transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);
            }
            else
            {
                transform.position = Vector3.Lerp(endPoint.position, startPoint.position, t);
            }

            // If the interpolation factor is greater than or equal to 1, the block has reached its destination, so we stop moving and reset the time elapsed
            if (t >= 1f)
            {
                isMoving = false;
                atPointA = !atPointA;
                timeElapsed = 0f;
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

    public override void DolphinInteract()
    {
        base.DolphinInteract();
        if (interactableByDolphin)
        {
            isMoving = true;
        }
    }


    #endregion

    #region BoxCasts
    private void FixedUpdate()
    {
        //canMoveNorth = Physics.BoxCast() WIP
    }

    #endregion

    #region DrawGizmos

    private void OnDrawGizmos()
    {
        transform.position +
    }

    #endregion

}
