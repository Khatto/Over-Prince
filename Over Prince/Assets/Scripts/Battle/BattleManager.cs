using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleState battleState = BattleState.NotStarted;
    public HPPortraitManager hpPortraitManager;
    public HPBar playerHPBar;
    public List<Enemy> enemies = new List<Enemy>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpPortraitManager = GameObject.FindGameObjectWithTag(Constants.TagKeys.PlayerHPPortrait).GetComponent<HPPortraitManager>();
        playerHPBar = GameObject.FindGameObjectWithTag(Constants.TagKeys.PlayerHPBar).GetComponent<HPBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBattle()
    {
        foreach (Enemy enemy in enemies)
        {
            enemy.StartBattle();
        }
        if (hpPortraitManager != null) hpPortraitManager.DisplayPortrait();
        if (playerHPBar != null) playerHPBar.FadeInHPBar();
        battleState = BattleState.InProgress;
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
    void OnBattleEnd();
}