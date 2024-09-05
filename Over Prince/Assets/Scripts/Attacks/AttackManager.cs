using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public GameObject hitboxPrefab;

    // TODO: Potentially convert this to be an int if we don't explicitly need the AttackID for other reasons
    public void PerformAttack(AttackID attackID, HitboxOwner hitboxOwner, Constants.Direction direction) {
        GenerateAttackHitboxes(attackID, hitboxOwner, direction);
    }

    /// <summary>
    /// Generates all hitboxes for the given attack.
    /// </summary>
    public void GenerateAttackHitboxes(AttackID attackID, HitboxOwner hitboxOwner, Constants.Direction direction) {
        Attack attack = AttackData.GetAttackByAttackID(attackID);
        for (int i = 0; i < attack.hits.Length; i++) {
            Debug.Log("Generating hitbox for hit " + i + " of " + attack.name + " for " + hitboxOwner + " in direction " + direction);
            GenerateHitboxForHit(attack, attack.hits[i], hitboxOwner, direction);
        }
    }

    private void GenerateHitboxForHit(Attack attack, Hit hit, HitboxOwner hitboxOwner, Constants.Direction direction) {
        GameObject hitbox = Instantiate(hitboxPrefab, GetHitboxPosition(attack), Quaternion.identity);
        HitboxManager hitboxManager = hitbox.GetComponent<HitboxManager>();
        hitboxManager.direction = direction;
        hitboxManager.hitboxOwner = hitboxOwner;
        hitboxManager.SetHitboxName();
        hitboxManager.PerformAttack(hit);
    }

    private Vector3 GetHitboxPosition(Attack attack) {
        float xOffset = attack.hitbox.xOffset * (transform.localScale.x > 0 ? 1 : -1);
        return new Vector3(transform.position.x + xOffset , transform.position.y + attack.hitbox.yOffset, 0);
    }

    public void DestroyInterruptibleHitboxes() {
        HitboxManager[] hitboxes = GetComponentsInChildren<HitboxManager>();
        foreach (HitboxManager hitbox in hitboxes) {
            if (hitbox.hit.isInterruptible) {
                Destroy(hitbox.gameObject);
            }
        }
    }
}