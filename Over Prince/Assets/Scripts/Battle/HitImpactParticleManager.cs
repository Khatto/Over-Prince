public class HitImpactParticleManager : CommonParticleGenerator
{
    public static HitImpactParticleManager instance;

    public override void SetupSingleton() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
}
