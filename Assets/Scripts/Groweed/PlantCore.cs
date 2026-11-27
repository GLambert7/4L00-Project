using FMODUnity;
using UnityEngine;

public class PlantCore : BaseInteractable
{
    // Inspector Vars
    [Tooltip("Add every Groweed that this plantcore controls to this array")]
    [SerializeField] private GameObject[] groweedsToFlip;
    [Tooltip("Does the plantcore's effect stop after a while?")]
    [SerializeField] private bool useResetTimer = true;
    [Tooltip("How long (In Seconds) does the plantcore's effect stay active?")]
    [SerializeField] private float resetTimer = 5f;

    public EventReference coreSound;

    // Non-Inspector Vars
    private float timeUntilReset = 0f;

    private void Update()
    {
        InteractionCheck();
        if (timeUntilReset > 0f) TimerTick(); 
    }

    public override void EelInteract()
    {
        base.EelInteract();
        Debug.Log("Eel Interact Called");
        timeUntilReset = resetTimer;
        RuntimeManager.PlayOneShot(coreSound, transform.position);
        FlipAll();
    }

    void TimerTick()
    {
        timeUntilReset -= Time.deltaTime;
        if (timeUntilReset < 0f) FlipAll(); 
    }

    void FlipAll()
    {
        Debug.Log("Flipall called");
        for (int i = 0; i < groweedsToFlip.Length; i++)
        {
            // I gotta stop making promises I can't keep ~ Everything Goes On: Porter Robinson
            Groweed isWeed = groweedsToFlip[i].GetComponent<Groweed>();

            if (isWeed != null)
            {
                isWeed.FlipGrowth();
            }
        }
    }

}
