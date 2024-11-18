using UnityEngine;

public class OrganicMouth : MonoBehaviour
{
    public SpriteRenderer mouthContainer;
    public SpriteRenderer mouthSprite; 
    public Sprite[] mouthSprites;
    public float speakSpeed = 0.1f;
    public Vector2 alignmentVelocity = new Vector2(0.0f, 0.0f);
    public float movementTime = 0.0f;
    public MoveBackAndForth moveBackAndForth;
    public MouthState mouthState = MouthState.Inactive;
    public Vector2 defaultPosition;

    private void Start() 
    {
        if (mouthContainer == null)
        {
            mouthContainer = GetComponent<SpriteRenderer>();
        }
        if (mouthSprite == null)
        {
            mouthSprite = mouthContainer.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        if (moveBackAndForth == null)
        {
            moveBackAndForth = gameObject.GetComponent<MoveBackAndForth>();
        }
        StartMouthAligningMovement();
    }

    public void StartMouthAligningMovement()
    {
        moveBackAndForth.SetValues(alignmentVelocity, movementTime);
        SetMoving(true);
    }

    public void ResetToDefaultPosition() {
        transform.localPosition = defaultPosition;
    }

    public void SetMoving(bool move) {
        if (move) {
            moveBackAndForth.StartMovement();
        } else {
            moveBackAndForth.StopMovement();
        }
    }
    
    void FixedUpdate()
    {
        if (mouthState == MouthState.Speaking && Time.time % speakSpeed < 0.01f)
        {
            RandomizeMouth();
        }
    }

    public void RandomizeMouth()
    {
        int randomMouth = Random.Range(1, mouthSprites.Length);
        mouthSprite.sprite = mouthSprites[randomMouth];
    }

    public void Speak(bool speak)
    {
        if (speak) {
            mouthState = MouthState.Speaking;
            mouthContainer.color = Constants.Colors.GetColorFullyVisible(mouthContainer.color);
            mouthSprite.color = Constants.Colors.GetColorFullyVisible(mouthSprite.color);
        } else {
            mouthState = MouthState.Inactive;
            mouthContainer.color = Constants.Colors.GetColorFullyTransparent(mouthContainer.color);
            mouthSprite.color = Constants.Colors.GetColorFullyTransparent(mouthSprite.color);
        }
    }
}

public enum MouthState
{
    Speaking,
    Inactive
}