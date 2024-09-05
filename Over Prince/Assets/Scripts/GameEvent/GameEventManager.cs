using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public GameEventManagerState state = GameEventManagerState.NotStarted;
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
        if (state != GameEventManagerState.Finished && other.CompareTag("Player"))
        {
            gameEventListener.OnGameEvent(gameEvent);
            state = GameEventManagerState.Finished;
        }
    }
}

public enum GameEventManagerState {
    NotStarted,
    Active,
    Finished
}

public enum GameEvent {
    BattleIntro
}