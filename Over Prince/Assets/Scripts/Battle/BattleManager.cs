using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleState battleState = BattleState.NotStarted;
    public Player player;
    public HPPortraitManager hpPortraitManager;
    public HPBar playerHPBar;
    public List<Enemy> enemies = new List<Enemy>();
    public IBattleEventListener battleEventListener;
    
    void Start()
    {
        hpPortraitManager = GameObject.FindGameObjectWithTag(Constants.TagKeys.PlayerHPPortrait).GetComponent<HPPortraitManager>();
        playerHPBar = GameObject.FindGameObjectWithTag(Constants.TagKeys.PlayerHPBar).GetComponent<HPBar>();
    }

    public void Setup(Player player, IBattleEventListener battleEventListener, List<Enemy> enemies = null) 
    {
        this.player = player;
        this.battleEventListener = battleEventListener;
        this.enemies = enemies;
    }

    // TODO: Check and see if it makes sense to have the player relay HP value changes instead of polling in Update
    void Update()
    {
        if (battleState == BattleState.InProgress) {
            CheckForPlayerHP();
            CheckForRemainingEnemies();
        }
    }

    public void StartBattle()
    {
        if (enemies == null || enemies.Count == 0) {
            Debug.LogError("No enemies to start battle with!");
            return;
        }
        foreach (Enemy enemy in enemies)
        {
            enemy.StartBattle();
        }
        ShowPlayerHP(true);
        battleState = BattleState.InProgress;
    }

    public void CheckForPlayerHP() 
    {
        if (player.stats.currentHP <= 0) {
            PlayerDied();
        }
    }

    public void CheckForRemainingEnemies() {
        enemies.RemoveAll(x => x == null);
        if (enemies.Count == 0) {
            CompleteBattle();
        }
    }

    public void PlayerDied() 
    {
        if (battleEventListener != null) battleEventListener.OnPlayerDied();
        foreach (Enemy enemy in enemies)
        {
            enemy.StopBattle();
        }
        EndBattle();
    }

    public void CompleteBattle() 
    {
        if (battleEventListener != null) battleEventListener.OnBattleComplete();
        EndBattle();
    }

    private void EndBattle() {
        float hpPortraitChangeDelay = HPPortraitManager.HPPortraitManagerConstants.portraitActionChangeTime * 2.0f + HPPortraitManager.HPPortraitManagerConstants.portraitFadeTime;
        StartCoroutine(DelayHPBarFade(hpPortraitChangeDelay));
        battleState = BattleState.Finished;
    }

    public void ShowPlayerHP(bool shouldDisplay) 
    {
        if (playerHPBar == null || hpPortraitManager == null) return;
        if (shouldDisplay) {
            hpPortraitManager.FadePortrait(FadeType.FadeIn);
            playerHPBar.FadeHPBar(FadeType.FadeIn);
        } else {
            hpPortraitManager.FadePortrait(FadeType.FadeOut);
            playerHPBar.FadeHPBar(FadeType.FadeOut);
        }
    }

    private IEnumerator DelayHPBarFade(float delay) {
        yield return new WaitForSeconds(delay);
        ShowPlayerHP(false);
    }
}

public enum BattleState {
    NotStarted,
    InProgress,
    Finished
}

public interface IBattleEventListener
{
    void OnBattleStart();
    void OnBattleComplete();
    void OnPlayerDied();
}