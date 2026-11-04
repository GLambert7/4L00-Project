using UnityEngine;

public class Generator : BaseInteractable
{
    [SerializeField] private GameObject[] poweredByGeneratorObjects;

    // Update is called once per frame
    void Update()
    {
        InteractionCheck();
    }

    public override void EelInteract()
    {
        base.EelInteract();

        if (interactableByEel)
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
