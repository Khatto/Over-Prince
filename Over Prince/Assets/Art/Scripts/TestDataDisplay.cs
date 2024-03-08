using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Analytics;

public class TestDataDisplay : MonoBehaviour
{

    public TextMeshProUGUI textMeshProUGUI;
    
    public Player player;
    public PlayerController playerController;
    public Animator animator;

    void Start()
    {
        textMeshProUGUI = gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (textMeshProUGUI != null) {
            string playerState = "Player Character State: " + player.state;
            string isJab3 = "Is in Jab-3: " + animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Attack-Jab3");
            textMeshProUGUI.text = playerState + "\n" + isJab3;
        }
    }
}
