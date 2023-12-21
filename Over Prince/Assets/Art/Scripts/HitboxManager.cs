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
    public Boolean displayHitbox = false;

    // Test Rendering Colors

    private Color startUpColor = new Color(1, 1, 1, 0.1f);

    private Color activeColor = new Color(1, 0, 0, 1.0f);

    private Color coolDownColor = new Color(0, 0, 1, 1);

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
            return (hit.startupFrames + hit.activeFrames + hit.coolDownFrames) / Constants.targetFPS;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        state = HitboxState.Inactive;
    }

    public void PerformAttack(Hit hit) {
        this.hit = hit;
        state = HitboxState.Startup;
    }


    // Update is called once per frame
    void Update()
    {
        switch (state) {
            case HitboxState.Startup:
            {
                timer += Time.deltaTime;
                if (timer >= computedStartupFrames) {
                    SetState(HitboxState.Active);
                }
                break;
            }
            case HitboxState.Active:
            {
                timer += Time.deltaTime;
                if (timer >= computedActiveFrames) {
                    SetState(HitboxState.CoolDown);
                }
                break;
            }
            case HitboxState.CoolDown:
            {
                timer += Time.deltaTime;
                if (timer >= computedCoolDownFrames) {
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
                spriteRenderer.color = startUpColor;
                break;
            }
            case HitboxState.Active:
            {
                spriteRenderer.color = activeColor;
                break;
            }
            case HitboxState.CoolDown:
            {
                spriteRenderer.color = coolDownColor;
                break;
            }
            case HitboxState.Finished:
            {
                Destroy(gameObject);
                break;
            }
        }
        this.state = state;
    }
}


public enum HitboxState {
    Inactive,
    Startup,
    Active,
    CoolDown,
    Finished
}