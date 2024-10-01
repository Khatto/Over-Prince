public class EnemyDeathParticleGenerator : CommonParticleGenerator
{
    public static EnemyDeathParticleGenerator instance;

    public override void SetupSingleton() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
}
