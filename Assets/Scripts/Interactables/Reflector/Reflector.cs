using System;
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
    [SerializeField] private Material reflectMaterial;
    #endregion

    [SerializeField] private bool isReflective = false;
    private bool currentlyReflecting = false;
    Renderer matSwitch;

    // Box Cast Variables
    [SerializeField] public float MaxCheckDistance = 0.8f;
    [SerializeField] public LayerMask TargetLayer;
    [SerializeField] public Vector3 boxSize = Vector3.one;
    [SerializeField] public bool drawBoxCasts = false;

    // Box Cast Non-Inspector Variables
    private GameObject northHit = null;
    private GameObject southHit = null;
    private GameObject eastHit = null;
    private GameObject westHit = null;
    private GameObject aboveHit = null;
    private GameObject belowHit = null;

    private bool reflectReceiveNorth = false;
    private bool reflectReceiveEast = false;
    private bool reflectReceiveSouth = false;
    private bool reflectReceiveWest = false;
    private bool reflectReceiveUp = false;
    private bool reflectReceiveDown = false;

    private bool reflectSendNorth = false;
    private bool reflectSendEast = false;
    private bool reflectSendSouth = false;
    private bool reflectSendWest = false;
    private bool reflectSendUp = false;
    private bool reflectSendDown = false;

    [SerializeField] public GameObject northLightEmitter;
    [SerializeField] public GameObject eastLightEmitter;
    [SerializeField] public GameObject southLightEmitter;
    [SerializeField] public GameObject westLightEmitter;
    [SerializeField] public GameObject aboveLightEmitter;
    [SerializeField] public GameObject belowLightEmitter;


    #region Unity Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        matSwitch = GetComponent<Renderer>();
        InitializeReflections();
        SetMaterial();
    }

    // Update is called once per frame
    void Update()
    {
        InteractionCheck();
        HandleReflections();
    }

    public void InitializeReflections()
    {
        for (int i = 0; i < recievingDirections.Length; i++)
        {
            switch (recievingDirections[i])
            {
                case Direction.North:
                    reflectReceiveNorth = true;
                    break;

                case Direction.East:
                    reflectReceiveEast = true;
                    break;

                case Direction.South:
                    reflectReceiveSouth = true;
                    break;

                case Direction.West:
                    reflectReceiveWest = true;
                    break;

                case Direction.Above:
                    reflectReceiveUp = true;
                    break;

                case Direction.Below:
                    reflectReceiveDown = true;
                    break;

                default:
                    // Runs if none of the cases above match
                    Debug.Log("No recieving direction set on " + this + "! Please set a direction for this object using the inspector.");
                    break;

            }
        }

        EmitterEnable(false);
    }

    public void HandleReflections()
    { 
        // When registering a hit from a recieving direction (small boxcast checking for if it's been hit), that flips a variable if it hits the right spot, and if that is true, we emit the light object from the set direction(s) from inspector
        // Light object is short and self-replicating for ease of variable lengths. Checks if it's hit something w a raycast, and if not, duplicates itself at the end of it's reach and continues until hitting something
        if (isReflective)
        {
            if (reflectReceiveNorth)
            {
                boxCastChecker(Vector3.forward, ref northHit); // North Check
            }
            if (reflectReceiveEast)
            {
            
                boxCastChecker(Vector3.right, ref eastHit); // East Check
            }
            if (reflectReceiveSouth)
            {
                boxCastChecker(Vector3.back, ref southHit); // South Check
            }
            if (reflectReceiveWest)
            {
                boxCastChecker(Vector3.left, ref westHit); // West Check
            }
            if (reflectReceiveUp)
            {
                boxCastChecker(Vector3.up, ref aboveHit); // Above Check
            }
            if (reflectReceiveDown)
            {
                boxCastChecker(Vector3.down, ref belowHit); // Below Check
            }

            if (northHit || eastHit || southHit || westHit || aboveHit || belowHit)
            {
                currentlyReflecting = true;
            }
            else
            {
                currentlyReflecting = false;

                northHit = null;
                eastHit = null;
                southHit = null;
                westHit = null;
                aboveHit = null;
                belowHit = null;
            }

            if (currentlyReflecting && isReflective)
            {
                if (reflectSendNorth)
                {
                    northLightEmitter.SetActive(true);
                }
                if (reflectSendEast)
                {
                    eastLightEmitter.SetActive(true);
                }
                if (reflectSendSouth)
                {
                    southLightEmitter.SetActive(true);
                }
                if (reflectSendWest)
                {
                    westLightEmitter.SetActive(true);
                }
                if (reflectSendUp)
                {
                    aboveLightEmitter.SetActive(true);
                }
                if (reflectSendDown)
                {
                    belowLightEmitter.SetActive(true);
                }
            }
            else
            {
                EmitterEnable(false);
            }
        }
        
        if (isReflective && currentlyReflecting)
        {
            for (int i = 0; i < sendingDirections.Length; i++)
            {
                switch (sendingDirections[i])
                {
                    case Direction.North:
                        reflectSendNorth = true;
                        break;

                    case Direction.East:
                        reflectSendEast = true;
                        break;

                    case Direction.South:
                        reflectSendSouth = true;
                        break;

                    case Direction.West:
                        reflectSendWest = true;
                        break;

                    case Direction.Above:
                        reflectSendUp = true;
                        break;

                    case Direction.Below:
                        reflectSendDown = true;
                        break;

                    default:
                        // Runs if none of the cases above match
                        Debug.Log("No sending direction set on " + this + "! Please set a direction for this object using the inspector.");
                        break;
                }
            }
        }
        else
        {
            for (int i = 0; i < sendingDirections.Length; i++)
            {
                switch (sendingDirections[i])
                {
                    case Direction.North:
                        reflectSendNorth = false;
                        break;

                    case Direction.East:
                        reflectSendEast = false;
                        break;

                    case Direction.South:
                        reflectSendSouth = false;
                        break;

                    case Direction.West:
                        reflectSendWest = false;
                        break;

                    case Direction.Above:
                        reflectSendUp = false;
                        break;

                    case Direction.Below:
                        reflectSendDown = false;
                        break;

                    default:
                        // Runs if none of the cases above match
                        Debug.Log("No sending direction set on " + this + "! Please set a direction for this object using the inspector.");
                        break;
                }
            }
        }
    }
    
    public void EmitterEnable(bool enable)
    {
        if (northLightEmitter != null) northLightEmitter.SetActive(enable);
        if (eastLightEmitter != null) eastLightEmitter.SetActive(enable);
        if (southLightEmitter != null) southLightEmitter.SetActive(enable);
        if (westLightEmitter != null) westLightEmitter.SetActive(enable);
        if (aboveLightEmitter != null) aboveLightEmitter.SetActive(enable);
        if (belowLightEmitter != null) belowLightEmitter.SetActive(enable);

    }
    public override void OctopusInteract()
    {
        base.OctopusInteract();
        if (interactableByOctopus)
        {
            isReflective = !isReflective;

            if (!isReflective)
            {
                currentlyReflecting = false;
                northHit = null;
                southHit = null;
                eastHit = null;
                westHit = null;
                aboveHit = null;
                belowHit = null;
                EmitterEnable(false);
            }

            SetMaterial();
        }
    }

    private void SetMaterial()
    {
        if (isReflective)
        {
            matSwitch.material = reflectMaterial;
        }
        else
        {
            matSwitch.material = nonReflectMaterial;
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
        bool reflectionDetection = Physics.BoxCast(
            origin, // position the cast starts from
            halfExtents, // how big the cast area is (half of the size of our box)
            direction, // Which way we're casting the box
            out RaycastHit hit, // The Raycast
            boxRotation, // In case the box is rotated this will correct it
            MaxCheckDistance, // How far is being checked
            TargetLayer, // What layer are we looking for?
            QueryTriggerInteraction.Collide // Should we hit triggers?
        );

        hitLocation = reflectionDetection ? hit.collider.gameObject : null;

        if (drawBoxCasts)
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying) { 
                Gizmos.color = reflectionDetection ? UnityEngine.Color.red : UnityEngine.Color.green;

                // Draw Start Box
                DrawBox(origin, boxSize, boxRotation);

                // Draw the end box, plus the line to it from start pos
                float distance = reflectionDetection ? hit.distance : MaxCheckDistance;
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
        boxCastChecker(Vector3.up, ref aboveHit); // Above Check
        boxCastChecker(Vector3.down, ref belowHit); // Below Check

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
