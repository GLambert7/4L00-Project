
using UnityEngine;

public class GeneratorPoweredObject : BaseInteractable
{
    [SerializeField] private GameObject objectToActivate;

    // Choose what happens when the generator powers this object
    [SerializeField] private bool disableInstead = false;

    public override void Activate()
    {
        base.Activate();

        if (objectToActivate != null)
        {
            if (disableInstead)
            {
                objectToActivate.SetActive(false);
            }
            else
            {
                objectToActivate.SetActive(true);
            }
        }
    }
}
