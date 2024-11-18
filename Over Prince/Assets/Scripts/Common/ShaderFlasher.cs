using UnityEngine;
using System.Collections;

public class ShaderFlasher : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer
    public Material solidOutlineFullColorMaterial; // Material to switch to temporarily
    public Material solidOutlineMaterial; // Material to switch back to
    public float flashTime = 1.0f; // Duration of the flash
    
    // TODO: Consider automatically loading the specicfied materials from the Resources folder

    void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    public void Flash()
    {
        StartCoroutine(FlashCoroutine());
    }

    public void Flash(float time)
    {
        flashTime = time;
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        // Switch to the full color material
        spriteRenderer.material = solidOutlineFullColorMaterial;

        // Wait for the specified flash time
        yield return new WaitForSeconds(flashTime);

        // Switch back to the original material
        spriteRenderer.material = solidOutlineMaterial;
    }
}