
using UnityEngine;

public class Groweed : MonoBehaviour
{
    [SerializeField] private float growthSpeed = 0.5f;
    [SerializeField] private float startingSize = 0.2f;
    [SerializeField] private float maxSize = 1f;

    private float currentSize;
    private bool receivingLight;
    private Vector3 startingPosition;
    private float originalHeight;

    private void Start()
    {
        //this is so we can move the plant as it grows
        startingPosition = transform.localPosition;

        //looks kinda complicated but basically it is finding the mesh attached to the plant and finds its size and stuff to grow it
        originalHeight = GetComponent<MeshFilter>().sharedMesh.bounds.size.y;

        currentSize = startingSize;
        transform.localScale = new Vector3(1f, currentSize, 1f);
    }

    private void Update()
    {
        if (receivingLight && currentSize < maxSize)
        {
            currentSize += growthSpeed * Time.deltaTime;

            //makes sure that the plant never goes over the max size
            currentSize = Mathf.Min(currentSize, maxSize);

            //these following 3 lines are for naking the polant grow upwards while moving the object so it looks normal
            transform.localScale = new Vector3(1f, currentSize, 1f);

            float heightDifference = (currentSize - startingSize) * originalHeight;

            transform.localPosition = startingPosition + Vector3.up * (heightDifference / 2f);
        }

        // Reset every frame unless the light hits again
        receivingLight = false;
    }

    public void ReceiveLight()
    {
        receivingLight = true;
    }
}
