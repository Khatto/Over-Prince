using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxManager : MonoBehaviour
{
    public List<HitboxOwner> hitboxesToWatchFor = new List<HitboxOwner>();
    private Character character;
    private IHurtboxTriggerListener hurtboxTriggerListener;
    private HitImpactParticleManager hitImpactParticleManager;

    void Start() {
        character = transform.parent.gameObject.GetComponent<Character>();
    }
    
    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == Constants.TagKeys.Hitbox) {
            HitboxManager manager = other.gameObject.GetComponent<HitboxManager>();
            if (manager.state == HitboxState.Active && hitboxesToWatchFor.Contains(manager.hitboxOwner) && CanBeHit()) {
                Debug.Log("(OnTriggerStay2D) " + transform.parent.name + " was hit! " + Time.time);
                ApplyAttackFromHitbox(other);
                if (hurtboxTriggerListener != null) hurtboxTriggerListener.OnHurtboxTriggerEnter(character);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == Constants.TagKeys.Hitbox) {
            HitboxManager manager = other.gameObject.GetComponent<HitboxManager>();
            if (manager.state == HitboxState.Active && hitboxesToWatchFor.Contains(manager.hitboxOwner)) {
                Log("TRIGGER ENTER! " + Time.time);
                ApplyAttackFromHitbox(other);
                if (hurtboxTriggerListener != null) hurtboxTriggerListener.OnHurtboxTriggerEnter(character);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == Constants.TagKeys.Hitbox) {
            HitboxManager manager = other.gameObject.GetComponent<HitboxManager>();
            if (manager.state == HitboxState.Active && hitboxesToWatchFor.Contains(manager.hitboxOwner)) {
                Log("TRIGGER EXIT! " + Time.time);
                if (hurtboxTriggerListener != null) hurtboxTriggerListener.OnHurtboxTriggerExit(character);
            }
        }
    }

    private void ApplyAttackFromHitbox(Collider2D other) {
        var manager = other.gameObject.GetComponent<HitboxManager>();
        Debug.Log(character.transform.name + " was in the " + character.state + " state when hit by " + manager.name + " at " + Time.time + " in the state: " + manager.state + " with color: " + manager.spriteRenderer.color);
        character.ApplyHit(manager.hit, manager.direction);
        PlayHitImpactParticles(other.transform.position);
    }

    private void PlayHitImpactParticles(Vector3 position) {
        if (hitImpactParticleManager == null) {
            hitImpactParticleManager = HitImpactParticleManager.instance;
        }
        hitImpactParticleManager.PlayHitImpactParticles(position);
    }

    private void Log(string message) {
        if (Settings.DisplayHitboxLogs) {
            Debug.Log(message);
        }
    }

    public void SetListener(IHurtboxTriggerListener listener) {
        hurtboxTriggerListener = listener;
    }

    bool CanBeHit() {
        return character.CanBeHit();
    }
}