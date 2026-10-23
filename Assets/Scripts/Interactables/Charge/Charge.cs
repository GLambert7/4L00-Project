using UnityEngine;

public class Charge : BaseInteractable
{
    [SerializeField] private Material unchargedMaterial;
    [SerializeField] private Material chargedMaterial;
    [SerializeField] private GameObject[] poweredObjects; // Array of objects that are powered by this charge object

    private Renderer objectRenderer;
    private bool isCharged = false;

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
    }

    public override void EelInteract()
    {
        base.EelInteract();

        //Check if the eel is allowed to interact with this object
        if (interactableByEel)
        {
            //marks object as interacted to avoid multiple triggers
            isCharged = !isCharged;

            //CHanges the material depending on if object is charged or uncharged
            objectRenderer.material = isCharged
                ? chargedMaterial
                : unchargedMaterial;
            Debug.Log(gameObject.name + " state: " + (isCharged ? "Charged" : "Uncharged"));
        }
    }
}
