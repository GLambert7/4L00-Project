using UnityEngine;

public class GroweedShrunkSpawner : MonoBehaviour
{
    public Groweed[] weeds;
    public GameObject payload;
    public GameObject instantiationLocation;
    public bool primedForPayload;
    public bool canSpawnObject = true;
    public bool requirementFulfilled = false;

    // Update is called once per frame
    void Update()
    {
        foreach (Groweed weed in weeds)
        {
            if (weed.transform.localScale.y >= weed.startingSize + .5f)
            {
                requirementFulfilled = false;
                break;
            }
            requirementFulfilled = true;
        }

        if (requirementFulfilled && primedForPayload && payload != null && canSpawnObject)
        {
            canSpawnObject = false;
            Instantiate(payload, instantiationLocation.transform.position, instantiationLocation.transform.rotation);
        }
    }
}
