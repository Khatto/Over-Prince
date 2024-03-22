using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxManager : MonoBehaviour
{
    public List<HitboxOwner> hitboxesToWatchFor = new List<HitboxOwner>();
    
    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == Constants.TagKeys.Hitbox) {
            HitboxManager manager = other.gameObject.GetComponent<HitboxManager>();
            if (manager.state == HitboxState.Active && hitboxesToWatchFor.Contains(manager.hitboxOwner)) {
                Log("TRIGGER STAY! " + Time.time);
                
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

    private void Log(string message) {
        if (Settings.DisplayHitboxLogs) {
            Debug.Log(message);
        }
    }

    bool CanBeHit() {
        return true;
    }
}
