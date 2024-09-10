using UnityEngine;

public class GameEventInstance : MonoBehaviour
{
    public GameEventInstanceState state = GameEventInstanceState.NotStarted;
    public GameEvent gameEvent;
    public GameplayScene gameplayScene;
    public IGameEventListener gameEventListener;
    public BoxCollider2D boxCollider2D;

    public void Start()
    {
        gameEventListener = gameplayScene.GetComponent<IGameEventListener>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (state != GameEventInstanceState.Finished && other.CompareTag("Player"))
        {
            gameEventListener.OnGameEvent(gameEvent);
            state = GameEventInstanceState.Finished;
        }
    }
}

public enum GameEventInstanceState {
    NotStarted,
    Active,
    Finished
}

public enum GameEvent {
    BattleIntro
}