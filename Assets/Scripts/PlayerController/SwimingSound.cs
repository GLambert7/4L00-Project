using FMODUnity;
using UnityEngine;

public class SwimingSound : MonoBehaviour
{
    [SerializeField] EventReference swimSoundEvent;
    [SerializeField] float rate;
    [SerializeField] GameObject player;
    [SerializeField] PlayerController1Script playerController;

    float time;
    public void PlaySwimSound()
    {
        RuntimeManager.PlayOneShotAttached(swimSoundEvent, player);
    }

    void Update()
    {
        time += Time.deltaTime;
        if (playerController.isMoving)
        {
            if (time >= rate)
            {
                PlaySwimSound();
                time = 0;
            }
        }
    }
}
