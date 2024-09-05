using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Manages the lifecycle of a generated Hitbox
/// </summary>
public class HitboxManager : MonoBehaviour
{
    public bool displayLogs = false;
    public Hit hit;
    public HitboxState state = HitboxState.Startup;
    public SpriteRenderer spriteRenderer;
    public bool displayHitbox = false;
    public HitboxOwner hitboxOwner = HitboxOwner.Player;
    public Constants.Direction direction;

    public float timeEnteredStartup;
    public float timeEnteredActive;
    public float timeEnteredCooldown;

    // Test Rendering Colors

    private Color startUpColor {
        get {
            return displayHitbox ? Constants.Colors.hitboxStartUpColor : Constants.Colors.transparent;
        }
    }

    private Color activeColor {
        get {
            return displayHitbox ? Constants.Colors.hitboxActiveColor : Constants.Colors.transparent;
        }
    }

    private Color coolDownColor {
        get {
            return displayHitbox ? Constants.Colors.hitboxCoolDownColor : Constants.Colors.transparent;
        }
    }

    public float timer = 0;

    private float computedStartupFrames {
        get {
            return hit.startupFrames / Constants.targetFPS;
        }
    }

    private float computedActiveFrames {
        get {
            return (hit.startupFrames + hit.activeFrames) / Constants.targetFPS;
        }
    }

    private float computedCoolDownFrames {
        get {
            return (hit.totalFrames - (hit.startupFrames + hit.activeFrames)) / Constants.targetFPS;
        }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        state = HitboxState.Inactive;
    }

    public void PerformAttack(Hit hit) {
        this.hit = hit;
        SetState(HitboxState.Startup);
    }

    public void SetHitboxName() {
        name = hitboxOwner.ToString() + "'s Hitbox-" + Constants.GenerateRandomDigits();
    }


    void FixedUpdate()
    {
        timer += Time.deltaTime;
        switch (state) {
            case HitboxState.Startup:
            {
                if (timer >= computedStartupFrames) {
                    SetState(HitboxState.Active);
                }
                break;
            }
            case HitboxState.Active:
            {
                if (timer >= computedActiveFrames) {
                    SetState(HitboxState.CoolDown);
                }
                break;
            }
            case HitboxState.CoolDown:
            {
                if (timer >= hit.totalFrames / Constants.targetFPS) {
                    SetState(HitboxState.Finished);
                }
                break;
            }
        
        }
    }

    void SetState(HitboxState state) {
        switch (state) {
            case HitboxState.Startup:
            {
                Debug.Log(name + " is in Startup at time " + Time.time);
                spriteRenderer.color = startUpColor;
                timeEnteredStartup = Time.time;
                break;
            }
            case HitboxState.Active:
            {
                Debug.Log(name + " is now Active at time " + Time.time);
                spriteRenderer.color = activeColor;
                timeEnteredActive = Time.time;
                break;
            }
            case HitboxState.CoolDown:
            {
                Debug.Log(name + " is now in CoolDown at time " + Time.time);
                spriteRenderer.color = coolDownColor;
                timeEnteredCooldown = Time.time;
                break;
            }
            case HitboxState.Finished:
            {
                if (displayLogs) {
                    Debug.Log("************************************");
                    Debug.Log("         F I N I S H E D");
                    Debug.Log("Startup Time was: " + timeEnteredStartup + " and ended at: " + timeEnteredActive);
                    Debug.Log("The duration of time spent in Startup was: " + (timeEnteredActive - timeEnteredStartup));
                    Debug.Log("The intended Startup time was: " + hit.startupFrames / Constants.targetFPS);
                    Debug.Log("Active Time was: " + timeEnteredActive + " and ended at: " + timeEnteredCooldown);
                    Debug.Log("The duration of time spent in Active was: " + (timeEnteredCooldown - timeEnteredActive));
                    Debug.Log("The intended Active time was: " + hit.activeFrames / Constants.targetFPS);
                    Debug.Log("************************************");
                }
                Destroy(gameObject);
                break;
            }
        }
        this.state = state;
    }

    private void Log(string message) {
        if (Settings.DisplayHitboxLogs) {
            Debug.Log(message);
        }
    }
}


public enum HitboxState {
    Inactive,
    Startup,
    Active,
    CoolDown,
    Finished
}