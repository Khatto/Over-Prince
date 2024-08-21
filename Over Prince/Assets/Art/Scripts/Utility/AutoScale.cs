using UnityEngine;
using UnityEngine.UIElements;

public class AutoScale : MonoBehaviour
{
    public bool matchViewportWidth = false;
    public bool matchViewportHeight = false;

    public bool useBaseHeight = false;
    public float baseHeight;
    public bool continuouslyScale = false;

    private void Start()
    {
        if (continuouslyScale)
        {
            SetScale();
        }
    }

    public void SetScale() {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float worldScreenHeight = (useBaseHeight ? baseHeight : Camera.main.orthographicSize) * 2.0f;
            
            if (matchViewportWidth) {
                float worldScreenWidth = worldScreenHeight * Camera.main.aspect;
                transform.localScale = new Vector3(worldScreenWidth / spriteRenderer.size.x, transform.localScale.y, transform.localScale.z);
            }
            if (matchViewportHeight) {
                transform.localScale = new Vector3(transform.localScale.x, worldScreenHeight / spriteRenderer.size.y, transform.localScale.z);
            }
        }
    }
}
