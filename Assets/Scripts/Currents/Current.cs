using UnityEngine;

public class Current : MonoBehaviour
{
    [Tooltip("How fast should the current push the objects inside?")]
    [SerializeField] private float currentSpeed = 1.0f;
    [Tooltip("Which layers can the current affect?")]
    [SerializeField] private string affectedByCurrent;

    [SerializeField] private bool isActive = true;

private void OnTriggerEnter(Collider other)
{
    Debug.Log("Detected: " + other.name);

    if (isActive)
    {
        if (other.CompareTag(affectedByCurrent))
        {
            Debug.Log("Moving: " + other.name);

            other.transform.position += (transform.rotation * Vector3.up) * currentSpeed * Time.fixedDeltaTime;
        }
    }
}

    public void switchState(bool state)
    {
        isActive = state;
    }
}
