using UnityEngine;


/*
 * This script is a WIP, if we decide we want interactions to be done with a Raycast instead of a trigger collider, then this script will be the one to handle that and I will build it out. Other than that, this script is currently not in use, but I will keep it here in case we want to implement it in the future.
 */
public class InteractionHandler : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float _interactionRange = 5f;
    [SerializeField] private LayerMask _interactionLayerMask;

    [Header("Player Input Handler")]
    [SerializeField] private PlayerInputHandler _inputHandler;


    /*To implement:
     * Layermask for interactable objects (so we can filter out what the player can interact with)
     * Interaction with objects (like picking up items, opening doors, clearing trash)
     * Interaction with NPCs (talking to them, triggering events, assuming we do that)
     * Interaction with the environment (like activating switches, selecting where to place the dudes)
     * 
     * If there's anything else you want me to add, please let me know and I will do my best to implement it.
    */


// Start is called once before the first execution of Update after the MonoBehaviour is created (You know what Start() is lol)
void Start()
    {
        
    }

    // Update is called once per frame (You know what Update() is lol)
    void Update()
    {
        
    }
}
