public static class Extensions {
    public static bool IsJab(this Attack attack) {
        return attack.attackID == AttackID.Jab;
    }
}