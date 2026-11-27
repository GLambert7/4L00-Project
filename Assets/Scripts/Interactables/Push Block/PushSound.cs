using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class PushSound : MonoBehaviour
{
    public PushBlock pushBlock;

    public EventReference pushSound;

    private EventInstance loopSoundPushInstance;

    private bool isPlayingSwimLoopSound = false;
    void Update()
    {
        if (pushBlock.IsMoving)
        {
            if (!isPlayingSwimLoopSound)
            {
                loopSoundPushInstance = FMODUnity.RuntimeManager.CreateInstance(pushSound);

                loopSoundPushInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
                FMODUnity.RuntimeManager.AttachInstanceToGameObject(loopSoundPushInstance, gameObject);

                loopSoundPushInstance.start();

                isPlayingSwimLoopSound = true;
            }
        }
        else
        {
            if (isPlayingSwimLoopSound)
            {
                loopSoundPushInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                loopSoundPushInstance.release();

                isPlayingSwimLoopSound = false;
            }
        }
    }
}
