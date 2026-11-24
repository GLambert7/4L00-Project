using UnityEngine;

public class BreakableBlock : BaseInteractable
{
    [Tooltip("Does this breakable block drop another item when broken?")]
    [SerializeField] private bool dropsItemOnBreak = false;
    [Tooltip("The item that is dropped when the block is broken.")]
    [SerializeField] private GameObject payload;

    public override void DolphinInteract()
    {
        base.DolphinInteract();
        if (interactableByDolphin)
        {
            HandleBreaking();
        }
    }

    private void HandleBreaking()
    {
        // Implement breakable block logic here
        if (dropsItemOnBreak && payload != null)
        {
            Instantiate(payload, transform.position, transform.rotation);
        }
        this.gameObject.SetActive(false);
    }
}
