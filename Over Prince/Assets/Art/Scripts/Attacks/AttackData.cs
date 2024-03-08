using System;
using UnityEditor.UI;


/// <summary>
/// Holds all of the data for the various attacks in the game
/// </summary>
public static class AttackData {

    public static class AnimationKeys {
        public static string Jab2 = "Base.Attack-Jab2";
        public static string Jab3 = "Base.Attack-Jab3";
    }

    private static Attack Jab = new Attack(
        AttackID.Jab,
        "Jab", 
        AttackTrigger.Single, 
        new Hit[] {
            new(30, 5, 10, 20, 3, 1, 0.0f)
        },
        new Hitbox(0.82f, 1.39f)
    );

    private static Attack Jab2 = new Attack(
        AttackID.Jab2,
        "Jab2", 
        AttackTrigger.Single, 
        new Hit[] {
            new(30, 4, 10, 20, 2, 1, 0.0f)
        },
        new Hitbox(0.85f, 1.42f)
    );

    private static Attack Jab3 = new Attack(
        AttackID.Jab3,
        "Jab3", 
        AttackTrigger.Single, 
        new Hit[] {
            new(30, 5, 4, 20, 3, 2, 1.0f)
        },
        new Hitbox(1.01f, 1.19f)
    );

    public static Attack GetAttackByAttackID(AttackID attackID) {
        switch (attackID) {
            case AttackID.Jab:
            {
                return Jab;
            }
            case AttackID.Jab2:
            {
                return Jab2;
            }
            case AttackID.Jab3:
            {
                return Jab3;
            }
            default:
            {
                return Jab;
            }
        }
    }

}

public class Attack {
    public AttackID attackID;
    public string name = "";
    public AttackTrigger trigger = AttackTrigger.Multi;

    /// <summary>
    /// The various hits that can occur during the attack.  Each can have it's own hitbox, damage, and frame data.
    /// </summary>
    public Hit[] hits;
    public Attack(AttackID attackID, string name, AttackTrigger trigger, Hit[] hits, Hitbox hitbox) {
        this.attackID = attackID;
        this.name = name;
        this.trigger = trigger;
        this.hits = hits;
        this.hitbox = hitbox;
    }
    public Hitbox hitbox;
}

// TODO: Do we need coolDownFrames?  Are can they be computed as totalFrames - startupFrames + activeFrames?
public class Hit {
    public int totalFrames;
    public int startupFrames;
    public int activeFrames;
    public int cancelleableFrame;
    public int hitStunFrames;
    public float damage;

    public float knockdownPower;

    public Hit(int totalFrames, int startupFrames, int activeFrames, int cancelleableFrame, int hitStunFrames, float damage, float knockdownPower) {
        this.totalFrames = totalFrames;
        this.startupFrames = startupFrames;
        this.activeFrames = activeFrames;
        this.cancelleableFrame = cancelleableFrame;
        this.hitStunFrames = hitStunFrames;
        this.damage = damage;
        this.knockdownPower = knockdownPower;
    }
}

/// <summary>
/// Describes whether or not the Attack will require multiple button/interaction presses or one.
/// </summary>
public enum AttackTrigger {
    Single,
    Multi
}

public enum AttackState {
    NotAttacking,
    Active
}

public enum AttackID {
    Jab = 1,
    Jab2 = 2,
    Jab3 = 3
}
