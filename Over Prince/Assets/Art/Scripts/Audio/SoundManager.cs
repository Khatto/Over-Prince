
using UnityEngine;

public class SoundManager : MonoBehaviour, IAnimationEventListener
{

    public static SoundManager instance;

    public AudioSource confirmSound;
    public AudioSource cancelSound;

    private struct SoundManagerConstants {
        public const string confirmChildName = "Confirm Sound";
        public const string cancelChildName = "Cancel Sound";
    }

    private void Awake()
    {
        SetupSingleton();
    }

    public void SetupSingleton() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        SetupSounds();

        DontDestroyOnLoad(gameObject);
    }

    public void SetupSounds() {
        if (confirmSound == null) {
            confirmSound = gameObject.transform.Find(SoundManagerConstants.confirmChildName).GetComponent<AudioSource>();
        }

        if (cancelSound == null) {
            cancelSound = gameObject.transform.Find(SoundManagerConstants.cancelChildName).GetComponent<AudioSource>();
        }
    }

    public void PlaySound(SoundType soundType)
    {
        switch (soundType)
        {
            case SoundType.Confirm:
                confirmSound.Play();
                break;
            case SoundType.Cancel:
                cancelSound.Play();
                break;
        }
    }

    public void OnAnimationEvent(AnimationEvent animationEvent)
    {
        switch (animationEvent)
        {
            case AnimationEvent.LeftFootstep:
                PlaySound(SoundType.Confirm);
                break;
            case AnimationEvent.RightFootstep:
                PlaySound(SoundType.Confirm);
                break;
            case AnimationEvent.ProtagLyingToStandingFinished:
                PlaySound(SoundType.Confirm);
                break;
            case AnimationEvent.ProtagSideTurnToFrontFinished:
                PlaySound(SoundType.Confirm);
                break;
        }
    }
}

public enum SoundType
{
    Confirm,
    Cancel
}
