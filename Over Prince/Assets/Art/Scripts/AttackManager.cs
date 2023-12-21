using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public GameObject hitboxPrefab;

    // TODO: Potentially convert this to be an int if we don't explicitly need the AttackID for other reasons
    public void PerformAttack(AttackID attackID) {
        GenerateAttackHitboxes(attackID);
    }

    /// <summary>
    /// Generates all hitboxes for the given attack.
    /// </summary>
    public void GenerateAttackHitboxes(AttackID attackID) {
        Attack attack = AttackData.GetAttackByAttackID(attackID);
        for (int i = 0; i < attack.hits.Length; i++) {
            GenerateHitboxForHit(attack, attack.hits[i]);
        }
    }

    private void GenerateHitboxForHit(Attack attack, Hit hit) {
        GameObject hitbox = Instantiate(hitboxPrefab, GetHitboxPosition(attack), Quaternion.identity);
        hitbox.GetComponent<HitboxManager>().PerformAttack(hit);
    }

    private Vector3 GetHitboxPosition(Attack attack) {
        float xOffset = attack.hitbox.xOffset * (transform.localScale.x > 0 ? 1 : -1);
        return new Vector3(transform.position.x + xOffset , transform.position.y + attack.hitbox.yOffset, 0);
    }
}