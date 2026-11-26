using FMOD;
using FMOD.Studio;
using FMODUnity;
using Mono.Cecil.Cil;
using UnityEngine;

public class SwimingSound : MonoBehaviour
{
    public PlayerController1Script playerController;

    public EventReference waterSwimSound;
    public EventReference waterDashSound;

    private EventInstance loopSoundSwimInstance;

    private bool isPlayingSwimLoopSound = false;
    private bool wasDashingLastFrame;
    void Update()
    {
        if (playerController.isMoving)
        {
            if (!isPlayingSwimLoopSound)
            {
                loopSoundSwimInstance = FMODUnity.RuntimeManager.CreateInstance(waterSwimSound);

                loopSoundSwimInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(loopSoundSwimInstance, gameObject);

                loopSoundSwimInstance.start();

                isPlayingSwimLoopSound = true;
            }
        }
        else
        {
            if (isPlayingSwimLoopSound)
            {
                loopSoundSwimInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                loopSoundSwimInstance.release();

                isPlayingSwimLoopSound = false;
            }
        }

        if (playerController.justDashed && !wasDashingLastFrame && playerController.isMoving)
        {
            RuntimeManager.PlayOneShot(waterDashSound, transform.position);
        }

        wasDashingLastFrame = playerController.justDashed;
    }
}

