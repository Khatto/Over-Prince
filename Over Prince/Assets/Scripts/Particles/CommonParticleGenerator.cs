using UnityEngine;

public class CommonParticleGenerator : MonoBehaviour
{
    public ParticleSystem particleGenerator;


    private void Awake()
    {
        SetupSingleton();
        SetupParticles();
        DontDestroyOnLoad(gameObject);
    }

    public virtual void SetupSingleton() {
        /* Implemented in child classes */
    }

    public void SetupParticles() {
        if (particleGenerator == null) {
            particleGenerator = gameObject.GetComponent<ParticleSystem>();
        }
    }

    public virtual void GenerateParticles(Vector3 position) {
        particleGenerator.transform.position = position;
        particleGenerator.Play();
    }
}