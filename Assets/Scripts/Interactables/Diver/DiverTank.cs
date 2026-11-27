using UnityEngine;

public class DiverTank : BaseInteractable
{
    [SerializeField] private GameObject requiredBlock;
    [SerializeField] private GameObject diver;
    [SerializeField] private float launchSpeed = 5f;

    private bool blockInPlace = false;
    private bool launching = false;

    private void OnTriggerEnter(Collider other)
    {
        // Player interaction detection
        if (other.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Detect the required block
        if (other.gameObject == requiredBlock)
        {
            blockInPlace = true;
            Debug.Log("Block detected");
        }
    }

    public override void DolphinInteract()
    {
        base.DolphinInteract();

        // Launch if the block has been detected
        if (blockInPlace && interactableByDolphin && script.canUseDolphin)
        {
            launching = true;
            Debug.Log("Launching diver");
        }
    }

    private void Update()
    {
        InteractionCheck();

        // Move the diver upwards
        if (launching)
        {
            diver.transform.position += Vector3.up * launchSpeed * Time.deltaTime;
        }
    }
}