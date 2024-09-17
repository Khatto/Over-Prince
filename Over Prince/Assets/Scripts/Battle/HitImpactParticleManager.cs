using UnityEngine;

public class HitImpactParticleManager : MonoBehaviour
{
    public static HitImpactParticleManager instance;

    public ParticleSystem hitImpactParticleSystem;

    private void Awake()
    {
        SetupSingleton();
    }

    public void SetupSingleton() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        SetupParticles();

        DontDestroyOnLoad(gameObject);
    }

    public void SetupParticles() {
        if (hitImpactParticleSystem == null) {
            hitImpactParticleSystem = gameObject.GetComponent<ParticleSystem>();
        }
    }

    public void PlayHitImpactParticles(Vector3 position) {
        hitImpactParticleSystem.transform.position = position;
        hitImpactParticleSystem.Play();
    }
}
