using Unity.VisualScripting;
using UnityEngine;

public class Current : MonoBehaviour
{
    [Tooltip("How fast should the current push the objects inside?")]
    [SerializeField] private float currentSpeed = 1.0f;
    [Tooltip("Which layers can the current affect?")]
    [SerializeField] private string affectedByCurrent;

    [SerializeField] public bool isActive = true;
    //this is so the current can start deactivated and appear when you "solve" the puzzle
    [Tooltip("Put the current gameobject here")]
    [SerializeField] private GameObject currentVisuals;

    public virtual void OnTriggerStay(Collider other)
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

    public virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("Detected Entering: " + other.name);
        PushBlock pushblock = other.gameObject.GetComponent<PushBlock>();
        if (pushblock != null)
        {
            pushblock.currentChange();
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        Debug.Log("Detected Exiting: " + other.name);
        PushBlock pushblock = other.gameObject.GetComponent<PushBlock>();
        if (pushblock != null)
        {
            pushblock.currentChange();
        }
    }

    public void switchState(bool state)
    {
        isActive = state;
        if (currentVisuals != null)
        {
            currentVisuals.SetActive(state);
        }
    }
}
