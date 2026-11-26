using UnityEngine;

public class CompanionLimiter : BaseInteractable
{
    public bool limitDolphin = false;
    public bool limitOctopus = false;
    public bool limitEel = false;

    public override void DolphinInteract()
    {
        base.DolphinInteract();
        if (interactableByDolphin && limitDolphin)
        {
            PlayerController1Script script = PlayerRef.GetComponent<PlayerController1Script>();

            if (script != null)
            {
                script.interactToggle(0); // Toggle Dolphin
            }
        }
    }
    public override void OctopusInteract()
    {
        base.OctopusInteract();
        if (interactableByOctopus && limitOctopus)
        {
            PlayerController1Script script = PlayerRef.GetComponent<PlayerController1Script>();

            if (script != null)
            {
                script.interactToggle(1); // Toggle Octopus
            }
        }
    }
    public override void EelInteract()
    {
        base.EelInteract();
        if (interactableByEel && limitEel)
        {
            PlayerController1Script script = PlayerRef.GetComponent<PlayerController1Script>();

            if (script != null)
            {
                script.interactToggle(2); // Toggle Eel
            }
        }
    }
}
