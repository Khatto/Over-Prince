using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxManager : MonoBehaviour
{
    public List<HitboxOwner> hitboxesToWatchFor = new List<HitboxOwner>();
    private Character character;

    void Start() {
        character = transform.parent.gameObject.GetComponent<Character>();
    }
    
    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == Constants.TagKeys.Hitbox) {
            HitboxManager manager = other.gameObject.GetComponent<HitboxManager>();
            if (manager.state == HitboxState.Active && hitboxesToWatchFor.Contains(manager.hitboxOwner) && CanBeHit()) {
                Log("Was hit! " + Time.time);
                ApplyAttackFromHitbox(other.GetComponent<HitboxManager>());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == Constants.TagKeys.Hitbox) {
            HitboxManager manager = other.gameObject.GetComponent<HitboxManager>();
            if (manager.state == HitboxState.Active && hitboxesToWatchFor.Contains(manager.hitboxOwner)) {
                Log("TRIGGER ENTER! " + Time.time);
                
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == Constants.TagKeys.Hitbox) {
            HitboxManager manager = other.gameObject.GetComponent<HitboxManager>();
            if (manager.state == HitboxState.Active && hitboxesToWatchFor.Contains(manager.hitboxOwner)) {
                Log("TRIGGER EXIT! " + Time.time);
                
            }
        }
    }

    private void ApplyAttackFromHitbox(HitboxManager manager) {
        character.ApplyHit(manager.hit, manager.direction);
    }

    private void Log(string message) {
        if (Settings.DisplayHitboxLogs) {
            Debug.Log(message);
        }
    }

    bool CanBeHit() {
        return character.CanBeHit();
    }
}
