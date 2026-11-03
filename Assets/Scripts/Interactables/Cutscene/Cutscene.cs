using UnityEngine;
using UnityEngine.Video;

public class VideoInteractable : BaseInteractable
{
    [SerializeField] private VideoPlayer videoPlayer;

    private void Start()
    {//makes sure the video player settings are proper
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    public override void PlayerInteract()
    {
        if (!interactableByPlayer)
        {
            base.PlayerInteract();
            return;
        }

        base.PlayerInteract();

        if (videoPlayer != null && !videoPlayer.isPlaying)
        {
            videoPlayer.enabled = true;
            videoPlayer.Play();
        }
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        videoPlayer.Stop();
        videoPlayer.enabled = false;
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}