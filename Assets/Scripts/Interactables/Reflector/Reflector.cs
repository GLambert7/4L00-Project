using UnityEditor.Build;
using UnityEngine;

public class Reflector : BaseInteractable
{
    #region Variables
    private enum Direction
    {
        North,
        East,
        South,
        West,
        Above,
        Below
    }

    [SerializeField] private Direction[] recievingDirections;
    [SerializeField] private Direction[] sendingDirections;


    [SerializeField] private Material nonReflectMaterial;
    [SerializeField] private Material ReflectMaterial;
    #endregion

    private bool isReflective = false;
    private bool currentlyReflecting = false;

    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HandleReflections();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleReflections()
    {
        for (int i = 0; i < recievingDirections.Length; i++)
        {
            switch (recievingDirections[i])
            {
                case Direction.North:
                    Debug.Log("Awaiting reflection from the North");
                    // Detection Logic for this direction
                    break;

                case Direction.East:
                    Debug.Log("Awaiting reflection from the East");
                    // Detection Logic for this direction
                    break;

                case Direction.South:
                    Debug.Log("Awaiting reflection from the South");
                    // Detection Logic for this direction
                    break;

                case Direction.West:
                    Debug.Log("Awaiting reflection from the West");
                    // Detection Logic for this direction
                    break;

                case Direction.Above:
                    Debug.Log("Awaiting reflection from Above");
                    // Detection Logic for this direction
                    break;

                case Direction.Below:
                    Debug.Log("Awaiting reflection from Below");
                    // Detection Logic for this direction
                    break;

                default:
                    // Runs if none of the cases above match
                    Debug.Log("No recieving direction set on "+ this +"! Please set a direction for this object using the inspector.");
                    break;

            }
        }

        // When registering a hit from a recieving direction (small boxcast checking for if it's been hit), that flips a variable if it hits the right spot, and if that is true, we emit the light object from the set direction(s) from inspector
        // Light object is short and self-replicating for ease of variable lengths. Checks if it's hit something w a raycast, and if not, duplicates itself at the end of it's reach and continues until hitting something
        if (isReflective && currentlyReflecting)
        {
            for (int i = 0; i < sendingDirections.Length; i++)
            {
                switch (sendingDirections[i])
                {
                    case Direction.North:
                        Debug.Log("Sending reflection from the North");
                        // Sending Logic for this direction
                        break;

                    case Direction.East:
                        Debug.Log("Sending reflection from the East");
                        // Sending Logic for this direction
                        break;

                    case Direction.South:
                        Debug.Log("Sending reflection from the South");
                        // Sending Logic for this direction
                        break;

                    case Direction.West:
                        Debug.Log("Sending reflection from the West");
                        // Sending Logic for this direction
                        break;

                    case Direction.Above:
                        Debug.Log("Sending reflection from Above");
                        // Sending Logic for this direction
                        break;

                    case Direction.Below:
                        Debug.Log("Sending reflection from Below");
                        // Sending Logic for this direction
                        break;

                    default:
                        // Runs if none of the cases above match
                        Debug.Log("No sending direction set on " + this + "! Please set a direction for this object using the inspector.");
                        break;
                }
            }
        }
    }

    public override void OctopusInteract()
    {
        base.OctopusInteract();
    }
    #endregion
}
