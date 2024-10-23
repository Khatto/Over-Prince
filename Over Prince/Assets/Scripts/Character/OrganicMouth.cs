using UnityEngine;

public class OrganicMouth : MonoBehaviour
{
    public GameObject mouthContainer;
    public SpriteRenderer mouthSprite; 
    public Sprite[] mouthSprites;

    public float mouthSpeed = 0.1f;

    public bool randomizeMouth = false;

    private void Start() 
    {
        if (mouthContainer == null)
        {
            mouthContainer = gameObject;
        }
        if (mouthSprite == null)
        {
            mouthSprite = mouthContainer.transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
    }
    
    void FixedUpdate()
    {
        if (randomizeMouth && Time.time % mouthSpeed < 0.01f)
        {
            RandomizeMouth();
        }
    }

    public void RandomizeMouth()
    {
        int randomMouth = Random.Range(1, mouthSprites.Length);
        mouthSprite.sprite = mouthSprites[randomMouth];
    }
}
