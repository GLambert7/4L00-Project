using FMODUnity;
using UnityEngine;

public class LiftableBlock : BaseInteractable
{
    // Public and Inspector Vars
    [Header("Lift Settings")]
    [Tooltip("How high do we want this block to be lifted?")]
    [SerializeField] private float howHighToLift = 2f; // The height to which the block will be lifted
    [Tooltip("How long does this block take to be lifted (In Seconds)?")]
    [SerializeField] private float howLongToLift = 2f; // The time it takes to lift the block

    [Header("Timer Settings")]
    [Tooltip("How long (In Seconds) can the octopus hold the block before dropping it back down?")]
    [SerializeField] private float holdTime = 5f; // The time to hold the block before it drops
    [Tooltip("Does the block drop back down after the hold time is up?")]
    [SerializeField] private bool dropsAfterHoldTime = true; // Whether the block drops after the hold time is up
    [Tooltip("At what rate does the block drop back down after the hold time is up? (In Seconds)")]
    [SerializeField] private float dropRate = 2f; // The rate at which the block drops back down

    public EventReference liftSound;

    // Private Vars
    private float groundHeight;
    private float liftedHeight;
    private float targetHeight;

    private float holdTimer = 0f; // Timer to track how long the block has been held

    private bool isLifted = false; // To track if the block is currently lifted

    private Vector3 targetPos;

    public override void Awake()
    {
        base.Awake();
        groundHeight = transform.position.y;
        liftedHeight = groundHeight + howHighToLift;
    }

    public void Update()
    {
        InteractionCheck();
        TickTimer();
        HandleLifting();
    }

    public override void OctopusInteract()
    {
        base.OctopusInteract();
        Debug.Log("Interacted with a liftable block!");
        if (interactableByOctopus)
        {
            RuntimeManager.PlayOneShot(liftSound, transform.position);
            ToggleLift();
        }
    }

    public void HandleLifting()
    {
        if (isLifted)
        {
            interactionArea.radius = 0.1f;
            targetHeight = liftedHeight;
            // Smoothly move the block to the target height
            targetPos = transform.position;
            targetPos.y = Mathf.Lerp(targetPos.y, targetHeight, Time.deltaTime / howLongToLift);
            transform.position = targetPos;
        }
        else
        {
            targetHeight = groundHeight;
            // Smoothly move the block to the target height
            targetPos = transform.position;
            targetPos.y -= dropRate * Time.deltaTime; // Drop the block at a constant rate
            if (targetPos.y < groundHeight) { targetPos.y = groundHeight; }
            transform.position = targetPos;
        }

        if (groundHeight - 1 <= transform.position.y && transform.position.y <= groundHeight + 1) interactionArea.radius = interactableRange;


    }

    void TickTimer()
    {
        if (isLifted)
        {
            holdTimer += Time.deltaTime;
            if (holdTimer >= holdTime)
            {
                if (dropsAfterHoldTime)
                {
                    ToggleLift();
                }
                holdTimer = 0f;
            }
        }
    }

    void ToggleLift()
    {
        isLifted = !isLifted;
    }
}
