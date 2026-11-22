using UnityEngine;

public class Current : MonoBehaviour
{
    [Tooltip("How fast should the current push the objects inside?")]
    [SerializeField] private float currentSpeed = 1.0f;
    [Tooltip("Which layers can the current affect?")]
    [SerializeField] private string affectedByCurrent;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(affectedByCurrent))
        {
            other.transform.position += (transform.rotation * Vector3.up) * currentSpeed * Time.fixedDeltaTime;
        }
    }
}
