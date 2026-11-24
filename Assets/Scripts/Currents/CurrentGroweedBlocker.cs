using UnityEngine;

public class CurrentGroweedBlocker : MonoBehaviour
{
    [SerializeField] private Current[] currents;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private Collider detectionArea;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerStay(Collider other)
    {
        if ((blockLayer.value & (1 << other.gameObject.layer)) == 0) return;
        foreach (Current obj in currents)
        {
            obj.switchState(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((blockLayer.value & (1 << other.gameObject.layer)) == 0) return;
        foreach (Current obj in currents)
        {
            obj.switchState(true);
        }
    }

}
