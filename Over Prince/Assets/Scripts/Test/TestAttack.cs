using UnityEngine;

public class TestAttack : MonoBehaviour
{

    public Vector2 knockback = new Vector2(0, 0);
    public Rigidbody2D rb;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Apply Knockback"))
        {
            rb.AddForce(knockback, ForceMode2D.Impulse);
        }
    }
}
