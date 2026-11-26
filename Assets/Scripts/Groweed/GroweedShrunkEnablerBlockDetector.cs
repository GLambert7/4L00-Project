using UnityEngine;

public class GroweedShrunkEnablerBlockDetector : MonoBehaviour
{
    public GroweedShrunkSpawner spawner;
    public BoxCollider deathcube;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerStay(Collider other)
    { 
        if (other.CompareTag("MoveBlocks"))
        {
            spawner.primedForPayload = true;
            Destroy(other);
        }
    }
}
