using FMOD;
using FMOD.Studio;
using FMODUnity;
using Mono.Cecil.Cil;
using UnityEngine;

public class SwimingSound : MonoBehaviour
{
    public PlayerController1Script playerController;

    public EventReference waterSwimSound;

    private EventInstance loopSoundInstance;

    private bool isPlayingLoopSound = false;
 void Update()
{
    if (playerController.isMoving)
    {
        if (!isPlayingLoopSound)
        {
        loopSoundInstance = FMODUnity.RuntimeManager.CreateInstance(waterSwimSound);

        loopSoundInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(loopSoundInstance, gameObject);

        loopSoundInstance.start();

        isPlayingLoopSound = true;
        }
    }
    else
    {
        if (isPlayingLoopSound)
        {
            loopSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            loopSoundInstance.release();

            isPlayingLoopSound = false;
        }
    }
}

}
