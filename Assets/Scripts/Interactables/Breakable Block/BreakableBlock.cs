using UnityEngine;

public class BreakableBlock : BaseInteractable
{
    public override void DolphinInteract()
    {
        base.DolphinInteract();
        if (interactableByDolphin)
        {
            // Implement breakable block logic here
            Debug.Log("Dolphin broke the block: " + gameObject.name);
            Destroy(gameObject);
        } 
    }
}
