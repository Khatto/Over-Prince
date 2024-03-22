using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the lifecycle of a generated Hitbox
/// </summary>
public class HitboxManager : MonoBehaviour
{

    public Hit hit;
    public HitboxState state = HitboxState.Startup;
    public SpriteRenderer spriteRenderer;
    public bool displayHitbox = false;
    public HitboxOwner hitboxOwner = HitboxOwner.Player;

    public float timeEnteredStartup;
    public float timeEnteredActive;
    public float timeEnteredCooldown;

    // Test Rendering Colors

    private Color startUpColor {
        get {
            return displayHitbox ? new Color(0, 1, 0, 1.0f) : Constants.Colors.transparent;
        }
    }

    private Color activeColor {
        get {
            return displayHitbox ? new Color(1, 0, 0, 1.0f) : Constants.Colors.transparent;
        }
    }

    private Color coolDownColor {
        get {
            return displayHitbox ? new Color(0, 0, 1, 1.0f) : Constants.Colors.transparent;
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


    void FixedUpdate()
    {
        switch (state) {
            case HitboxState.Startup:
            {
                timer += Time.deltaTime;
                Log("Hitbox is in Startup State: " + Time.time);
                if (timer >= computedStartupFrames) {
                    SetState(HitboxState.Active);
                }
                break;
            }
            case HitboxState.Active:
            {
                Log("Hitbox is in Active State: " + Time.time);
                timer += Time.deltaTime;
                if (timer >= computedActiveFrames) {
                    SetState(HitboxState.CoolDown);
                }
                break;
            }
            case HitboxState.CoolDown:
            {
                Log("Hitbox is in CoolDown State: " + Time.time);
                timer += Time.deltaTime;
                if (timer >= hit.totalFrames / Constants.targetFPS) {
                    SetState(HitboxState.Finished);
                }
                break;
            }
        
        }
    }

    void SetState(HitboxState state) {
        Log("==== Setting Hitbox State to: " + state.ToString() + " at " + Time.time + " ====");
        switch (state) {
            case HitboxState.Startup:
            {
                spriteRenderer.color = startUpColor;
                timeEnteredStartup = Time.time;
                break;
            }
            case HitboxState.Active:
            {
                spriteRenderer.color = activeColor;
                timeEnteredActive = Time.time;
                break;
            }
            case HitboxState.CoolDown:
            {
                spriteRenderer.color = coolDownColor;
                timeEnteredCooldown = Time.time;
                break;
            }
            case HitboxState.Finished:
            {
                Log("************************************");
                Log("         F I N I S H E D");
                Log("Startup Time was: " + timeEnteredStartup + " and ended at: " + timeEnteredActive);
                Log("The duration of time spent in Startup was: " + (timeEnteredActive - timeEnteredStartup));
                Log("The intended Startup time was: " + hit.startupFrames / Constants.targetFPS);
                Log("Active Time was: " + timeEnteredActive + " and ended at: " + timeEnteredCooldown);
                Log("The duration of time spent in Active was: " + (timeEnteredCooldown - timeEnteredActive));
                Log("The intended Active time was: " + hit.activeFrames / Constants.targetFPS);
                Log("************************************");
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