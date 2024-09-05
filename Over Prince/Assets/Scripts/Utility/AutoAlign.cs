using UnityEngine;

public class AutoAlign : MonoBehaviour
{
    public VerticalAlignment verticalAlignment = VerticalAlignment.Center;
    public HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center;
    public float verticalPadding = 0;
    public float horizontalPadding = 0;
    public bool continuouslyAdjustVertical = false;
    public bool continuouslyAdjustHorizontal = false;
    public bool adjustOnStart = true;

    void Start()
    {
        if (adjustOnStart)
        {
            AdjustVerticalAlignment();
            AdjustHorizontalAlignment();
        }
    }

    void Update()
    {
        if (continuouslyAdjustVertical)
        {
            AdjustVerticalAlignment();
        }
        if (continuouslyAdjustHorizontal)
        {
            AdjustHorizontalAlignment();
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

    public void AdjustHorizontalAlignment()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) {
            float spriteWidth = spriteRenderer.size.x * spriteRenderer.transform.localScale.x;
            float spriteX = transform.position.x;
            float spriteLeft = spriteX - spriteWidth / 2;
            float spriteRight = spriteX + spriteWidth / 2;
            float cameraWidth = Camera.main.aspect * Camera.main.orthographicSize * 2;
            float cameraLeft = Camera.main.transform.position.x - cameraWidth / 2;
            float cameraRight = Camera.main.transform.position.x + cameraWidth / 2;
            float xOffset = 0;
            switch (horizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    xOffset = cameraLeft - spriteLeft;
                    break;
                case HorizontalAlignment.Center:
                    xOffset = (cameraLeft + cameraRight) / 2 - (spriteLeft + spriteRight) / 2;
                    break;
                case HorizontalAlignment.Right:
                    xOffset = cameraRight - spriteRight;
                    break;
            }
            transform.position = new Vector3(transform.position.x + xOffset + horizontalPadding, transform.position.y, transform.position.z);
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