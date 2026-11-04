
using UnityEngine;

public class Groweed : MonoBehaviour
{
    [SerializeField] private float growthSpeed = 0.5f;
    [SerializeField] private float startingSize = 0.2f;
    [SerializeField] private float maxSize = 1f;
    [SerializeField] private LayerMask blockLayer;

    [Space(10)]
    [SerializeField] private bool canShrink = false; // Whether the plant can shrink when not receiving light

    private float currentSize;
    private bool receivingLight;
    private Vector3 startingPosition;
    private float originalHeight;

    private PushBlock liftedBlock;

    private void Start()
    {
        //this is so we can move the plant as it grows
        startingPosition = transform.localPosition;

        //looks kinda complicated but basically it is finding the mesh attached to the plant and finds its size and stuff to grow it
        originalHeight = GetComponent<MeshFilter>().sharedMesh.bounds.size.y;

        currentSize = startingSize;
        transform.localScale = new Vector3(transform.localScale.x, currentSize, transform.localScale.z);
    }

    private void Update()
    {
        if (!receivingLight && currentSize <= startingSize)
        {
            gameObject.layer = LayerMask.NameToLayer("GroWeedInactive");
        } else
        {
            gameObject.layer = LayerMask.NameToLayer("GroWeed");
        }

        if (receivingLight && currentSize < maxSize)
        {
            currentSize += growthSpeed * Time.deltaTime;

            //makes sure that the plant never goes over the max size
            currentSize = Mathf.Min(currentSize, maxSize);

            //these following 3 lines are for naking the polant grow upwards while moving the object so it looks normal
            transform.localScale = new Vector3(transform.localScale.x, currentSize, transform.localScale.z);

            float heightDifference = (currentSize - startingSize) * originalHeight;

            transform.localPosition = startingPosition + Vector3.up * (heightDifference / 2f);
        }
        else if (!receivingLight && canShrink && currentSize > startingSize)
        {
            currentSize -= growthSpeed * Time.deltaTime;
            //makes sure that the plant never goes under the min size
            currentSize = Mathf.Max(currentSize, startingSize);

            //What grayson said above, but down instead of up
            transform.localScale = new Vector3(transform.localScale.x, currentSize, transform.localScale.z);
            float heightDifference = (currentSize - startingSize) * originalHeight;
            transform.localPosition = startingPosition + Vector3.up * (heightDifference / 2f);
        }

        // Check for blocks above the plant, even when it isn't growing
        Renderer plantRenderer = GetComponent<Renderer>();
        Bounds plantBounds = plantRenderer.bounds;
        float plantTop = plantBounds.max.y;

        //choses where to look for blocks
        Vector3 checkPosition = new Vector3(
            plantBounds.center.x,
            plantTop,
            plantBounds.center.z
        );

        //makes an iunvisible detector box and return every collider that is overlaping with it (I think) so it looks for blocks on move blocks layer
        Collider[] blocks = Physics.OverlapBox(
            checkPosition,
            new Vector3(0.5f, 1f, 0.5f),
            Quaternion.identity,
            blockLayer
        );

        PushBlock detectedBlock = null;

        //this goes through all the coliders detected by the collider[] thing
        foreach (Collider block in blocks)
        {
            PushBlock pushBlock = block.GetComponentInParent<PushBlock>();

            if (pushBlock != null)
            {
                detectedBlock = pushBlock;
                break;
            }
        }

       //check the plant is holding a block
        if (liftedBlock != null)
        {
            //finds the blocks collider
            Collider blockCollider = liftedBlock.GetComponent<Collider>();

            if (blockCollider != null)
            {
                Bounds blockBounds = blockCollider.bounds;

                //this part checks if the block is still on top of the plant horizontally while ignoring its height
                bool horizontallyOverlapping =
                    blockBounds.max.x > plantBounds.min.x &&
                    blockBounds.min.x < plantBounds.max.x &&
                    blockBounds.max.z > plantBounds.min.z &&
                    blockBounds.min.z < plantBounds.max.z;

                //basically just tells the push block script the plant is no longer being l;ifted
                if (!horizontallyOverlapping)
                {
                    liftedBlock.SetBeingLifted(false);
                    liftedBlock = null;
                }
            }
        }

        if (liftedBlock == null && detectedBlock != null && !detectedBlock.IsMoving)
        {
            liftedBlock = detectedBlock;
        }

        if (liftedBlock != null && !liftedBlock.IsMoving)
        {
            // Prevent the block from falling through the plant
            liftedBlock.SetBeingLifted(true);

            // Find the top of the plant
            float currentPlantTop = plantRenderer.bounds.max.y;

            // Find half the height of the block
            Collider blockCollider = liftedBlock.GetComponent<Collider>();

            if (blockCollider != null)
            {
                float blockHalfHeight = blockCollider.bounds.extents.y;
                float blockBottom = liftedBlock.transform.position.y - blockHalfHeight;
                float heightAdjustment = currentPlantTop - blockBottom;

                // Place the bottom of the block on top of the plant
                Vector3 blockPosition = liftedBlock.transform.position;
                blockPosition.y += heightAdjustment;

                liftedBlock.transform.position = blockPosition;
            }
        }

        // Reset every frame unless the light hits again
        receivingLight = false;
    }

    public void ReceiveLight()
    {
        //Debug.Log("Plant received light! Current size: " + currentSize);
        receivingLight = true;
    }
}
