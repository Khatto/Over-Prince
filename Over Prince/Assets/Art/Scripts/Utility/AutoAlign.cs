using UnityEngine;

public class AutoAlign : MonoBehaviour
{
    public VerticalAlignment verticalAlignment = VerticalAlignment.Center;
    public HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center;
    public float verticalPadding = 0;
    public bool continuousAdjustment = false;

    void Start()
    {
        AdjustVerticalAlignment();
    }

    void Update()
    {
        if (continuousAdjustment)
        {
            AdjustVerticalAlignment();
        }
    }

    public void AdjustVerticalAlignment()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) {
            float spriteHeight = spriteRenderer.size.y * spriteRenderer.transform.localScale.y;
            float spriteY = transform.position.y;
            float spriteTop = spriteY + spriteHeight / 2;
            float spriteBottom = spriteY - spriteHeight / 2;
            float cameraHeight = Camera.main.orthographicSize * 2;
            float cameraTop = Camera.main.transform.position.y + cameraHeight / 2;
            float cameraBottom = Camera.main.transform.position.y - cameraHeight / 2;
            float yOffset = 0;
            switch (verticalAlignment)
            {
                case VerticalAlignment.Top:
                    yOffset = cameraTop - spriteTop;
                    break;
                case VerticalAlignment.Center:
                    yOffset = (cameraTop + cameraBottom) / 2 - (spriteTop + spriteBottom) / 2;
                    break;
                case VerticalAlignment.Bottom:
                    yOffset = cameraBottom - spriteBottom;
                    break;
            }
            transform.position = new Vector3(transform.position.x, transform.position.y + yOffset + verticalPadding, transform.position.z);

        }
    }
}

public enum VerticalAlignment {
    Top,
    Center,
    Bottom
}

public enum HorizontalAlignment {
    Left,
    Center,
    Right
}
