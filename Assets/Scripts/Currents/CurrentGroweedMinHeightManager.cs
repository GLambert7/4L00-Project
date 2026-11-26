using UnityEngine;

public class CurrentGroweedMinHeightManager : MonoBehaviour
{
    [SerializeField] private Groweed[] weeds;
    [SerializeField] private Current[] currents;

    private bool requirementFulfilled = false;

    private void Update()
    {
        foreach(Groweed weed in weeds)
        {
            if (weed.transform.localScale.y >= weed.startingSize + .5f)
            {
                requirementFulfilled = false;
                break;
            }
            requirementFulfilled = true;
        }

        if (requirementFulfilled)
        {
            foreach (Current current in currents)
            {
                current.isActive = false;
            }
        } else
        {
            foreach (Current current in currents)
            {
                current.isActive = true;
            }
        }
    }
}
