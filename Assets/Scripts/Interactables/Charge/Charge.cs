using UnityEngine;

public class Charge : BaseInteractable
{
    [SerializeField] private Material unchargedMaterial;
    [SerializeField] private Material chargedMaterial;

    private Renderer objectRenderer;
    private bool isCharged = false;
    private bool hasInteracted = false;

    void Start()
    {
        //Gets the texture and sets starting material
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material = unchargedMaterial;
    }
    // Update is called once per frame
    void Update()
    {
        //Checks if player is in range and interacting
        InteractionCheck();

        //Checks that the eel button has been let go before triggering again
        if (!_inputHandler.EelInteractInput)
        {
            hasInteracted = false;
        }
    }

    public override void EelInteract()
    {
        if (hasInteracted) return;

        base.EelInteract();

        //Check if the eel is allowed to interact with this object
        if (interactableByEel)
        {
            //marks object as interacted to avoid multiple triggers
            hasInteracted = true;
            isCharged = !isCharged;

            //CHanges the material depending on if object is charged or uncharged
            objectRenderer.material = isCharged
                ? chargedMaterial
                : unchargedMaterial;
            Debug.Log("Charged");
        }
    }
}
