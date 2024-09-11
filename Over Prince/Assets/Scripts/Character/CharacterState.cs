public enum CharacterState {
    Disengaged = -1,
    Idle = 0,
    Walking = 1,
    Running = 2,
    Attacking = 3,
    HitStun = 4,
    Invulnerable = 5,
    Dying = 6,
    Dead = 7
}

public enum SpecialCharacterType {
    None,
    IntroTutorial,
}

public enum SpecialCharacterAction {
    None,
    FaceCharacterAfterHitStun
}