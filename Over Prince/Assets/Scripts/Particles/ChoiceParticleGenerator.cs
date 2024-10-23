using UnityEngine;

public class ChoiceParticleGenerator : CommonParticleGenerator
{
    public Gradient frenzyGradient;
    public Gradient sorrowGradient;
    public Gradient confusionGradient;
    public Gradient whiteGradient;

    private bool hasSetupColors = false;

    public static class ChoiceParticleGeneratorConstants {
        public static GradientAlphaKey[] alphaKeys = new GradientAlphaKey[] {
            new GradientAlphaKey(0.0f, 0.0f),
            new GradientAlphaKey(1.0f, 0.35f),
            new GradientAlphaKey(1.0f, 0.65f),
            new GradientAlphaKey(0.0f, 1.0f)
        };
    }

    public void SetupColorGradients() {
        frenzyGradient = new Gradient();
        frenzyGradient.SetKeys(
            GenerateColorKeys(Constants.Colors.frenzyColor),
            ChoiceParticleGeneratorConstants.alphaKeys
        );
        sorrowGradient = new Gradient();
        sorrowGradient.SetKeys(
            GenerateColorKeys(Constants.Colors.sorrowColor),
            ChoiceParticleGeneratorConstants.alphaKeys
        );
        confusionGradient = new Gradient();
        confusionGradient.SetKeys(
            GenerateColorKeys(Constants.Colors.confusionColor),
            ChoiceParticleGeneratorConstants.alphaKeys
        );
        whiteGradient = new Gradient();
        whiteGradient.SetKeys(
            GenerateColorKeys(Color.white),
            ChoiceParticleGeneratorConstants.alphaKeys
        );
        Debug.Log("Frenzy Gradient at 0% is " + frenzyGradient.Evaluate(0.0f));
        Debug.Log("Frenzy Gradient at 10% is " + frenzyGradient.Evaluate(0.1f));
        Debug.Log("Frenzy Gradient at 50% is " + frenzyGradient.Evaluate(0.5f));
        Debug.Log("Frenzy Gradient at 90% is " + frenzyGradient.Evaluate(0.9f));
        Debug.Log("Frenzy Gradient at 100% is " + frenzyGradient.Evaluate(1.0f));

    }

    private GradientColorKey[] GenerateColorKeys(Color color) {
        return new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) };
    }

    public void GenerateParticles(Vector3 position, Constants.Emotions emotion) {
        if (!hasSetupColors) {
            SetupColorGradients();
            hasSetupColors = true;
        }
        SetRandomIntoWhiteColorOverLifetime(emotion);
        base.GenerateParticles(position);
    }

    public void SetRandomIntoWhiteColorOverLifetime(Constants.Emotions emotion)
    {
        var col = particleGenerator.colorOverLifetime;
        col.color = new Gradient()
        {
            colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(emotion switch
                {
                    Constants.Emotions.Frenzy => frenzyGradient.Evaluate(Random.value),
                    Constants.Emotions.Sorrow => sorrowGradient.Evaluate(Random.value),
                    Constants.Emotions.Confusion => confusionGradient.Evaluate(Random.value),
                    _ => Color.white
                }, 0.0f),
                new GradientColorKey(Color.white, 1.0f)
            },
            alphaKeys = ChoiceParticleGeneratorConstants.alphaKeys
        };
    }
}