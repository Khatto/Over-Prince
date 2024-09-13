using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleState battleState = BattleState.NotStarted;
    public List<Enemy> enemies = new List<Enemy>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
    }
}

public enum BattleState {
    NotStarted,
    InProgress,
    Finished
}