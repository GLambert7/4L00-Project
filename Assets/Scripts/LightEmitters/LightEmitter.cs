using UnityEngine;

public class LightEmitter : MonoBehaviour
{
    [SerializeField] public LineRenderer lightBeam;
    [SerializeField] public float maxDistance = 100f;
    [SerializeField] public Transform lightEmitLocation;
    [SerializeField] public GameObject lightOrb;
    [SerializeField] public float targetOffset = 1f;

    private bool hitSomething = false;

    private ChildOrb childOrb;

    //tracks the objects hit to avoid spawning like 100 orbs
    private Collider previousHit;

    private int layerMask;


    private void Awake()
    {
        layerMask = ~LayerMask.GetMask("Player", "PassThrough");
    }

    private void Update()
    {
        Ray lightRay = new Ray(transform.position, transform.up);
        RaycastHit hit;

        // Line start location
        lightBeam.SetPosition(0, transform.position);


        // Check for hits
        if (Physics.Raycast(lightRay, out hit, maxDistance, layerMask))
        {
            // Hit
            Vector3 targetPoint = hit.point + lightRay.direction * targetOffset;
            lightBeam.SetPosition(1, targetPoint);
            //checks if light is hitting a groweed
            Groweed plant = hit.collider.GetComponentInParent<Groweed>();

            //tell tjhe plant it is getting light
            if (plant != null)
            {
                plant.ReceiveLight();
            }
            // Only create a new orb when the hit object changes
            if (previousHit != hit.collider)
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
                previousHit = hit.collider;

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

    private void OnEnable()
    {
        if (lightBeam != null)
        {
            lightBeam.positionCount = 2;
            lightBeam.enabled = true;
        }

        previousHit = null;
    }

    private void OnDisable()
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
}
