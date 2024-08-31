using UnityEngine;

public class CharacterSoundManager : MonoBehaviour, IAnimationEventListener
{
    public AudioSource leftFootstepSound;
    public AudioSource rightFootstepSound;


    public void OnAnimationEvent(AnimationEvent animationEvent)
    {
        switch (animationEvent)
        {
            case AnimationEvent.LeftFootstep:
                leftFootstepSound.Play();
                break;
            case AnimationEvent.RightFootstep:
                rightFootstepSound.Play();
                break;
        }
    }
}