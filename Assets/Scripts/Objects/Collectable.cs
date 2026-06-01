using UnityEngine;
namespace Terraphibian
{
public class Collectable : MonoBehaviour
    {
        Rigidbody2D rb;

        private bool canBeCollected = true;
        private float collectTimer = 0f;
    
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (!canBeCollected)
            {
                collectTimer -= Time.deltaTime;
                if (collectTimer <= 0)                
                {
                    canBeCollected = true;
                }
            }
        }

        public void SetCollectDelay(float delay)
        {
            canBeCollected = false;
            collectTimer = delay;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!canBeCollected) return;
            if (collision.CompareTag("Player"))
            {
                // Here you can add logic to give the player the collectable item
                collision.GetComponent<Inventory>()?.AddSeeds(1);
                // Debug.Log("Collectable picked up!");
                Destroy(gameObject); // Remove the collectable from the scene
            }
        }

        public void Launch(Vector2 force)
        {
            rb.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
