using UnityEngine;

public class PuzzleManager : MonoBehaviour
{

    // This puzzle manager checks if the puzzle is complete. If you are looking for the script that checks if each creature can interact with each element, check the CreatureInteractionManager script.
    [SerializeField] private GameObject[] movingBlocks; // Array of moving blocks in the puzzle
    [SerializeField] private GameObject[] reflectiveSurfaces; // Array of reflective surfaces
    [SerializeField] private GameObject[] lightSources; // Array of light sources
    [SerializeField] private GameObject[] solarPanels; // Array of solar panels
    [SerializeField] private GameObject[] glass; // Array of glass objects
    [SerializeField] private GameObject[] seaweed; // Array of seaweed objects
    [SerializeField] private GameObject[] seaweedElevator; // Array of seaweed elevator objects


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
