using UnityEngine;

public class CurrentPushBlocks : Current
{
    private BoxCollider currentArea;
    public GameObject pushedFromCurrentSpot;
    private void Awake()
    {
        currentArea = GetComponent<BoxCollider>();
    }
    public override void OnTriggerStay(Collider other)
    {
        if (isActive)
        {
            if (other.isTrigger) return;
            PushBlock script = other.GetComponent<PushBlock>();
            if (script != null)
            {
                Vector3 difference = other.transform.position - currentArea.bounds.min;
                if (script.inCurrentMovement) return;

                script.PushHandling(pushedFromCurrentSpot, 0);

            }
        }
    }
}
