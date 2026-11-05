
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GroweedCutsceneTrigger : MonoBehaviour
{
    [SerializeField] private VideoPlayer cutscenePlayer;

    private bool hasTriggered = false;

    private void Start()
    {
        // Listen for when the cutscene finishes
        cutscenePlayer.loopPointReached += EndCutscene;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is on the Groweed layer
        if (other.gameObject.layer == LayerMask.NameToLayer("GroWeed") && !hasTriggered)
        {
            hasTriggered = true;

            // Play the cutscene
            cutscenePlayer.Play();
        }
    }

    private void EndCutscene(VideoPlayer player)
    {
        // Load the main menu when the video finishes
        SceneManager.LoadScene("Main Menu");
    }

    private void OnDestroy()
    {
      
        if (cutscenePlayer != null)
        {
            cutscenePlayer.loopPointReached -= EndCutscene;
        }
    }
}
