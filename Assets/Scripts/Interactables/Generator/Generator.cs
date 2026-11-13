using UnityEngine;

public class Generator : BaseInteractable
{
    [SerializeField] private GameObject[] poweredByGeneratorObjects;

    // The location where the generator needs to be to work
    [SerializeField] private Transform correctPosition;

    // How close the generator needs to be to that location
    [SerializeField] private float positionTolerance = 0.2f;

    // Update is called once per frame
    void Update()
    {
        InteractionCheck();
    }

    public override void EelInteract()
    {
        base.EelInteract();
        // Only power objects if the generator is in the correct position and interactable by the eel
        if (interactableByEel && correctPosition != null &&
            Vector3.Distance(transform.position, correctPosition.position) < positionTolerance)
        {
            foreach (GameObject obj in poweredByGeneratorObjects)
            {
                if (obj != null)
                {
                    BaseInteractable interactable = obj.GetComponent<BaseInteractable>();
                    if (interactable != null)
                    {
                        interactable.Activate();
                    }
                }
            }
        }
    }
}
