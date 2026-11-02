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

        if (!hitSomething)
        {
            // Check for hits
            if (Physics.Raycast(lightRay, out hit, maxDistance, layerMask))
            {
                // Hit
                Vector3 targetPoint = hit.point + lightRay.direction * targetOffset;
                lightBeam.SetPosition(1, targetPoint);
                GameObject orbForDetection = Instantiate(lightOrb, targetPoint, Quaternion.identity);
                childOrb = orbForDetection.GetComponent<ChildOrb>();

                if (childOrb != null)
                {
                    childOrb.Initialize(this);
                }

                hitSomething = true;
            }
            else
            {
                // no hit
                lightBeam.SetPosition(1, lightRay.GetPoint(maxDistance));
            }
        }
        

    }

    private void OnEnable()
    {
        if (lightBeam != null)
        {
            lightBeam.positionCount = 2;
            lightBeam.enabled = true;
        }

        hitSomething = false;
    }

    private void OnDisable()
    {
        if (lightBeam != null)
        {
            lightBeam.enabled = false;
            lightBeam.positionCount = 0;
        }

        hitSomething = false;

        if (childOrb != null)
        {
            Destroy(childOrb.gameObject);
            childOrb = null;
        }
    }
}
