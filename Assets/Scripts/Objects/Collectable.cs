using UnityEngine;

public class Collectable : MonoBehaviour
{
    Rigidbody2D rb;
   
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Here you can add logic to give the player the collectable item
            Debug.Log("Collectable picked up!");
            Destroy(gameObject); // Remove the collectable from the scene
        }
    }

    public void Launch(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}
