using UnityEngine;

public class ChoiceParticleGenerator : CommonParticleGenerator
{
    public void GenerateParticles(Vector3 position, Constants.Emotions emotion) {
        ParticleSystem.MainModule main = particleGenerator.main;
        main.startColor = emotion switch {
            Constants.Emotions.Frenzy => Color.red,
            Constants.Emotions.Sorrow => Color.blue,
            Constants.Emotions.Confusion => Color.green,
            _ => Color.white
        };
        base.GenerateParticles(position);
    }
}