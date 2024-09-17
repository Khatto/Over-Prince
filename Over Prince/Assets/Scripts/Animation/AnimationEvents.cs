using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] public GameObject listener;
    private IAnimationEventListener animationEventListener;

    private void Start()
    { 
        if (listener != null) {
            animationEventListener = listener.GetComponent<IAnimationEventListener>();
        }
    }

    internal void SendAnimationEvent(string animationEvent)
    {
        if (animationEventListener != null) {
            animationEventListener.OnAnimationEvent((AnimationEvent) AnimationEvent.Parse(typeof(AnimationEvent), animationEvent));
        }
    }
}

public enum AnimationEvent {
    LeftFootstep,
    RightFootstep,
    ProtagLyingToStandingFinished,
    ProtagSideTurnToFrontFinished,
    Slime,
    EnemyCry,
    EnemyCharge,
    AttackWhoosh
}