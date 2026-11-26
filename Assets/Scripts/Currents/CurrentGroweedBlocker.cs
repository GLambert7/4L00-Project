using UnityEngine;

public class CurrentGroweedBlocker : MonoBehaviour
{
    [Tooltip("Put current(s) you want to activate here")]
    [SerializeField] private Current[] currents;
    [Tooltip("Put groweeds for the puzzle here")]
    [SerializeField] private Groweed[] groweeds;
    [Tooltip("Put Breakable block here")]
    [SerializeField] private BreakableBlock breakableBlock;

    private void Update()
    {
        
        bool allWeedsShrunk = true;

    
        foreach (Groweed weed in groweeds)
        {
            if (!weed.IsShrunk)
            {
                allWeedsShrunk = false;
                break;
            }
        }
        //both conditions must be met to activate the current
        bool activateCurrent = allWeedsShrunk && breakableBlock.IsBroken;

        foreach (Current obj in currents)
        {
            obj.switchState(activateCurrent);
        }
        if (activateCurrent)
        {
            enabled = false;
        }
    }
}