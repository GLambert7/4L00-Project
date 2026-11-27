using UnityEngine;

public class BreakableDoor : MonoBehaviour
{
    [SerializeField] private GameObject breakingObject;
    [SerializeField] private GameObject krill;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Detected: " + other.gameObject.name);

        if (other.gameObject == breakingObject)
        {
            krill.SetActive(true);
            Destroy(gameObject);
        }
    }
}