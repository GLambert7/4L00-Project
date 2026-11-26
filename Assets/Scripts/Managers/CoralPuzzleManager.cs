using UnityEngine;

public class CoralPuzzleManager : MonoBehaviour
{
    [SerializeField] private BlockChecker[] blockCheckLocations;

    [SerializeField] private Current puzzleEndCurrent;

    private bool puzzleComplete;

    private void Awake()
    {
        puzzleEndCurrent.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        foreach (BlockChecker check in blockCheckLocations)
        {
            puzzleComplete = true;
            puzzleComplete = check.blockFound;
            if (!puzzleComplete) break;
        }

        if (puzzleComplete)
        {
            puzzleEndCurrent.gameObject.SetActive(true);
        }
    }
}
