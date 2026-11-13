using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VisionTrigger : MonoBehaviour
{
    //timer length
    [SerializeField] private float detectionTime = 3f;

    private Coroutine detectionCoroutine;

    //when the player enters the trigger collider
    private void OnTriggerEnter(Collider other)
    {
        //only effects objects tagged player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered vision!");
            
            //starts timer
            detectionCoroutine = StartCoroutine(DetectionCountdown());
        }
    }
    //when the player enters the trigger collider
    private void OnTriggerExit(Collider other)
    {
        //only effects objects tagged player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left vision!");

            if (detectionCoroutine != null)
            {
                //stops timer if you leave the collider
                StopCoroutine(detectionCoroutine);
                detectionCoroutine = null;
            }
        }
    }

    //this is the timer function
    private IEnumerator DetectionCountdown()
    {
        Debug.Log("Countdown started!");

        yield return new WaitForSeconds(detectionTime);

        Debug.Log("Player detected for too long!");

        //Resets scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        detectionCoroutine = null;
    }
}