using UnityEngine;

public class BreakableBlock : BaseInteractable
{
    [Tooltip("Does this breakable block drop another item when broken?")]
    [SerializeField] private bool dropsItemOnBreak = false;
    [Tooltip("The item that is dropped when the block is broken.")]
    [SerializeField] private GameObject payload;
    //so other scripts can reference the broken block for barn puzzle
    public bool IsBroken { get; private set; }

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
        IsBroken = true;
        this.gameObject.SetActive(false);
    }
}
