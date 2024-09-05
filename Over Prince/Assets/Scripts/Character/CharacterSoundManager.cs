using System;
using UnityEngine;

/// <summary>
/// The class that manages the specific sounds related to a character through listening to animation events and directly applying sounds.
/// </summary>
public class CharacterSoundManager : MonoBehaviour, IAnimationEventListener
{
    public AudioSource leftFootstepSound;
    public AudioSource rightFootstepSound;

    public AudioSource slime1Sound;
    public AudioSource slime2Sound;
    public AudioSource[] slimeSounds;

    public AudioSource enemyCry1Sound;
    public AudioSource enemyCry2Sound;
    public AudioSource enemyCry3Sound;
    public AudioSource[] enemyCrySounds;

    public AudioSource enemyChargeSound;

    public AudioSource lightHit1Sound;
    public AudioSource lightHit2Sound;
    public AudioSource lightHit3Sound;

    public AudioSource[] lightHitSounds;

    private void Start()
    {
        SetupGroupSounds();
    }

    private void SetupGroupSounds() {
        if (slime1Sound != null && slime2Sound != null)
        {
            slimeSounds = new AudioSource[] {slime1Sound, slime2Sound};
        }
        if (enemyCry1Sound != null && enemyCry2Sound != null && enemyCry3Sound != null)
        {
            enemyCrySounds = new AudioSource[] {enemyCry1Sound, enemyCry2Sound, enemyCry3Sound};
        }
        if (lightHit1Sound != null && lightHit2Sound != null && lightHit3Sound != null)
        {
            lightHitSounds = new AudioSource[] {lightHit1Sound, lightHit2Sound, lightHit3Sound};
        }
    }

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
            case AnimationEvent.Slime:
                PlayRandomSound(slimeSounds);
                break;
            case AnimationEvent.EnemyCry:
                PlayRandomSound(enemyCrySounds);
                break;
            case AnimationEvent.EnemyCharge:
                enemyChargeSound.Play();
                break;
        }
    }

    public void PlayDeathSound()
    {
        PlayRandomSound(enemyCrySounds);
    }

    public void PlayHitContactSound(HitContactSound hitContactSound)
    {
        switch (hitContactSound)
        {
            case HitContactSound.Light:
                PlayRandomSound(lightHitSounds);
                break;
            case HitContactSound.Medium:
                PlayRandomSound(lightHitSounds);
                break;
            case HitContactSound.Heavy:
                PlayRandomSound(lightHitSounds);
                break;

        }
    }

    private void PlayRandomSound(AudioSource[] audioSources)
    {
        int random = UnityEngine.Random.Range(0, audioSources.Length);
        audioSources[random].Play();
    }
}