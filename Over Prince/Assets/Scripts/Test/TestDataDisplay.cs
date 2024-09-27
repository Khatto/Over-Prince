using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Analytics;

public class TestDataDisplay : MonoBehaviour
{

    public TextMeshProUGUI textMeshProUGUI;
    
    public Player player;
    public Enemy triangleSlime;
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
            string triangleSlimeState = "Triangle Slime Character State: " + triangleSlime.state;
            string isJab3 = "Is in Jab-3: " + animator.GetCurrentAnimatorStateInfo(0).IsName("Base.Attack-Jab3");
            string fps = "FPS: " + (1 / Time.unscaledDeltaTime);
            string time = "Time: " + Time.time;
            textMeshProUGUI.text = playerState + "\n" + triangleSlimeState + "\n" + isJab3 + "\n" + fps + "\n" + time;
        }
    }
}
