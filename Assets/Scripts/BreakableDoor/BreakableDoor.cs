using UnityEngine;

public class BreakableDoor : MonoBehaviour
{
    // The object that can break the door
    [SerializeField] private GameObject breakingObject;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Detected: " + other.gameObject.name);

        // Check if the correct object has hit the door
        if (other.gameObject == breakingObject)
        {
            Destroy(gameObject);
        }
    }
}