using UnityEngine;

public class SolarPanel : BaseInteractable
{
    [SerializeField] private GameObject[] poweredByPanelObjects;
    [Space(10)]
    [SerializeField] public Vector3 boxSize = Vector3.one;
    [SerializeField] public float MaxCheckDistance = 0.8f; 
    [SerializeField] public LayerMask TargetLayer;
    [SerializeField] public bool drawBoxCasts = false;
    private GameObject PanelHit = null;
    private bool powered = false;


    private void FixedUpdate()
    {
        boxCastChecker(Vector3.up, ref PanelHit); // Above Check
    }

    // Update is called once per frame
    void Update()
    {
        if (PanelHit != null)
        {
            powered = true;
        } 
        
        else         
        {
            powered = false;
        }

        if (powered)
        {
            foreach (GameObject obj in poweredByPanelObjects)
            {
                if (obj != null)
                {
                    BaseInteractable interactable = obj.GetComponent<BaseInteractable>();
                    if (interactable != null)
                    {
                        interactable.Activate();
                    }
                }
            }
        }
    }

    private void boxCastChecker(Vector3 localDirection, ref GameObject hitLocation)
    {
        Vector3 origin = transform.position;
        Vector3 halfExtents = boxSize * 0.5f;
        Quaternion boxRotation = transform.rotation;

        Collider[] hits = Physics.OverlapBox(
            origin,
            halfExtents,
            boxRotation,
            TargetLayer,
            QueryTriggerInteraction.Collide);

        hitLocation = hits.Length > 0 ? hits[0].gameObject : null;


        if (drawBoxCasts)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {

                Gizmos.color = hitLocation != null ? UnityEngine.Color.red : UnityEngine.Color.green;

                // Draw Start Box
                DrawBox(origin, boxSize, boxRotation);

                // Draw the end box, plus the line to it from start pos
                float distance = hitLocation != null ? Vector3.Distance(origin, hitLocation.transform.position) : MaxCheckDistance;
                Vector3 endPointCalc = origin + localDirection.normalized * distance;

                Gizmos.DrawLine(origin, endPointCalc);
                DrawBox(endPointCalc, boxSize, boxRotation);
            }
#endif

        }


    }

    #region DrawGizmos

    private void OnDrawGizmos()
    {
        boxCastChecker(Vector3.up, ref PanelHit); // Above Check
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
