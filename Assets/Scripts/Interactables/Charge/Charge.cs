using UnityEngine;

public class Charge : BaseInteractable
{
    [SerializeField] private Material unchargedMaterial;
    [SerializeField] private Material chargedMaterial;
    [SerializeField] private GameObject[] poweredObjects; // Array of objects that are powered by this charge object

    private Renderer objectRenderer;
    [SerializeField] private float chargeAmount = 0f;
    [SerializeField] private float maxChargeAmount = 5f; // Maximum charge amount
    [SerializeField] private float chargeRate = 1f; // Rate at which the charge increases
    [SerializeField] private float dischargeRate = 1f; // Rate at which the charge decreases

    void Start()
    {
        //Gets the texture and sets starting material
        objectRenderer = GetComponent<Renderer>();
        objectRenderer.material = unchargedMaterial;
    }
    // Update is called once per frame
    void Update()
    {
        chargeAmount -= Time.deltaTime * dischargeRate;
        if (chargeAmount < 0f) { chargeAmount = 0f; }
        else if (chargeAmount > maxChargeAmount) { chargeAmount = maxChargeAmount; }
        switchPowerState();
        //Checks if player is in range and interacting
        InteractionCheck();
    }

    public void switchPowerState()
    {
        if (chargeAmount > 0f)
        {
            objectRenderer.material = chargedMaterial;
        }
        else
        {
            objectRenderer.material = unchargedMaterial;
        }
    }

    override public void Activate()
    {
        base.Activate();
        chargeAmount += Time.deltaTime * chargeRate;
    }
}
