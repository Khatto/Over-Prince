using UnityEngine;

/// <summary>
/// Holds all of the data for the various attacks in the game
/// </summary>
public static class AttackData {

    public static class AnimationKeys {
        public static string Jab2 = "Base.Attack-Jab2";
        public static string Jab3 = "Base.Attack-Jab3";
    }

    public static class HitStopDurations {
        public static float none = 0.0f;
        public static float light = 2f / Constants.targetFPS;
        public static float medium = 4f / Constants.targetFPS;
    }

    private static Attack Jab = new Attack(
        AttackID.Jab,
        "Jab", 
        AttackTrigger.Single, 
        new Hit[] {
            new(30, 5, 10, 20, 10, 1, 0.0f, new Vector2(0, 0), HitStopDurations.light)
        },
        new Hitbox(0.82f, 1.39f)
    );

    private static Attack Jab2 = new Attack(
        AttackID.Jab2,
        "Jab2", 
        AttackTrigger.Single, 
        new Hit[] {
            new(30, 5, 10, 20, 10, 1, 0.0f, new Vector2(0, 0), HitStopDurations.light)
        },
        new Hitbox(0.85f, 1.42f)
    );

    private static Attack Jab3 = new Attack(
        AttackID.Jab3,
        "Jab3", 
        AttackTrigger.Single, 
        new Hit[] {
            new(30, 5, 4, 20, 45, 3, 1.0f, new Vector2(1000.0f, 0), HitStopDurations.medium)
        },
        new Hitbox(1.05f, 1.19f, 1.5f, 1.5f)
    );

    private static Attack TriangleSlimeSlam = new Attack(
        AttackID.TriangleSlimeSlam,
        "TriangleSlimeSlam", 
        AttackTrigger.Single, 
        new Hit[] {
            new(40, 21, 15, 35, 15, 3, 0.0f, new Vector2(1000.0f, 0), HitStopDurations.light)
        },
        new Hitbox(1.06f, 1.56f, 1.5f, 1.5f)
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
            case AttackID.TriangleSlimeSlam:
            {
                return TriangleSlimeSlam;
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
    public int damage;
    public float knockdownPower;
    public Vector2 knockback;
    public float hitStopDuration = 0.0f;
    public bool isInterruptible = false;
    public HitContactSound hitContactSound;

    public Hit(int totalFrames, int startupFrames, int activeFrames, int cancelleableFrame, int hitStunFrames, int damage, float knockdownPower, Vector2 knockback, float hitStopDuration = 0.0f, bool isInterruptible = true, HitContactSound hitContactSound = HitContactSound.Light) {
        this.totalFrames = totalFrames;
        this.startupFrames = startupFrames;
        this.activeFrames = activeFrames;
        this.cancelleableFrame = cancelleableFrame;
        this.hitStunFrames = hitStunFrames;
        this.damage = damage;
        this.knockdownPower = knockdownPower;
        this.knockback = knockback;
        this.hitStopDuration = hitStopDuration;
        this.isInterruptible = isInterruptible;
        this.hitContactSound = hitContactSound;
    }
}

public enum HitContactSound {
    None,
    Light,
    Medium,
    Heavy
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
    Jab3 = 3,
    TriangleSlimeSlam = 4
}
