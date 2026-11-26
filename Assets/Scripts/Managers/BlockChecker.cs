using UnityEngine;

public class BlockChecker : MonoBehaviour
{
    public bool blockFound = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MoveBlocks")) blockFound = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MoveBlocks")) blockFound = false;
    }
}
