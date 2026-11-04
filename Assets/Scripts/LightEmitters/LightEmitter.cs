using System.Runtime.CompilerServices;
using UnityEngine;

public class LightEmitter : BaseInteractable
{
    [SerializeField] public LineRenderer lightBeam;
    [SerializeField] public float maxDistance = 100f;
    [SerializeField] public Transform lightEmitLocation;
    [SerializeField] public GameObject lightOrb;
    [SerializeField] public float targetOffset = 1f;

    [SerializeField] private bool isActive = false;
    private bool isActiveSecondary;

    private ChildOrb childOrb;

    //tracks the objects hit to avoid spawning like 100 orbs
    private Collider previousHit;

    private int layerMask;


    private void Awake()
    {
        isActiveSecondary = isActive;
        layerMask = ~LayerMask.GetMask("Player", "PassThrough", "MoveTargets", "LightEmitter");
    }

    void Update() 
    {
        InteractionCheck();
        if (isActive)
        {
            Ray lightRay = new Ray(transform.position, transform.up);
            RaycastHit[] hits = Physics.RaycastAll(lightRay, maxDistance, layerMask);
            System.Array.Sort(hits,(firstHit, secondHit) => firstHit.distance.CompareTo(secondHit.distance));

            // Line start location
            lightBeam.SetPosition(0, transform.position);


            // Check for hits
            if (hits.Length > 0)
            {
                foreach (RaycastHit hit in hits)
                {                
                    // Checks if light is hitting a groweed
                    Groweed plant = hit.collider.GetComponentInParent<Groweed>();

                    // Tell the plant it is getting light
                    if (plant != null)
                    {
                        plant.ReceiveLight();
                    }
                }

                // Hit
                Vector3 targetPoint = hits[0].point + lightRay.direction * targetOffset;
                lightBeam.SetPosition(1, targetPoint);
                
                // Only create a new orb when the hit object changes
                if (previousHit != hits[0].collider)
                {


                    if (childOrb != null)
                    {
                        Destroy(childOrb.gameObject);
                        childOrb = null;
                    }
                    //create an orb at new hit location
                    GameObject orbForDetection = Instantiate(lightOrb, targetPoint, Quaternion.identity);
                    childOrb = orbForDetection.GetComponent<ChildOrb>();

                    if (childOrb != null)
                    {
                        childOrb.Initialize(this);
                    }
                    //remember which object was hit
                    previousHit = hits[0].collider;

                }

                if (childOrb != null)
                {
                    childOrb.transform.position = targetPoint;
                }
            }
            else
            {
                // no hit
                lightBeam.SetPosition(1, lightRay.GetPoint(maxDistance));

                if (childOrb != null)
                {
                    Destroy(childOrb.gameObject);
                    childOrb = null;
                }

                previousHit = null;
            }
        }
    }

    public override void EelInteract()
    {
        base.EelInteract();

        if (interactableByEel)
        {
            isActive = !isActive;
            HandleActivity();
        }
    }
    private void OnEnable()
    {
        isActive = isActiveSecondary;
        HandleActivity();
    }

    private void OnDisable()
    {
        isActive = false;
        HandleActivity();
    }

    private void DisableLight()
    {
        if (lightBeam != null)
        {
            lightBeam.enabled = false;
            lightBeam.positionCount = 0;
        }

        previousHit = null;

        if (childOrb != null)
        {
            Destroy(childOrb.gameObject);
            childOrb = null;
        }
    }

    private void EnableLight()
    {
        if (lightBeam != null)
        {
            lightBeam.positionCount = 2;
            lightBeam.enabled = true;
        }

        previousHit = null;
    }

    private void HandleActivity()
    {
        if (isActive)
        {
            EnableLight();
        }
        else
        {
            DisableLight();
        }
    }

    public override void Activate()
    {
        base.Activate();
        isActive = !isActive;
        HandleActivity();
    }
}
