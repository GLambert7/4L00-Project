using UnityEngine;

public class PreventRotation : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (transform.rotation != Quaternion.identity)
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
