using UnityEngine;

namespace Terraphibian
{
    public class CollisionDataRetriever : MonoBehaviour
    {
        public bool OnGround { get; private set; }
        public bool OnWall { get; private set; }
        public float Friction { get; private set; }
        public Vector2 ContactNormal { get; private set; }
        
        private PhysicsMaterial2D _material;
        Controller _controller;

        private void Awake()
        {
            _controller = GetComponent<Controller>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            EvaluateCollision(collision);
            RetrieveFriction(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            EvaluateCollision(collision);
            RetrieveFriction(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            OnGround = false;
            Friction = 0;
            OnWall = false;
        }

        public void EvaluateCollision(Collision2D collision)
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                ContactNormal = collision.GetContact(i).normal;
                // Ground contact check
                OnGround |= ContactNormal.y >= 0.9f;

                // Wall contact check
                if (Mathf.Abs(ContactNormal.x) >= 0.9f)
                {
                    Debug.Log(ContactNormal.x);
                    bool wallOnLeft = ContactNormal.x > 0.9f;
                    bool wallOnRight = ContactNormal.x < -0.9f;

                    if ((!OnGround && wallOnRight && _controller.input.RetrieveMoveInput(this.gameObject) > 0f) ||
                        (!OnGround && wallOnLeft && _controller.input.RetrieveMoveInput(this.gameObject) < 0f))
                    {
                        OnWall = true;
                    }
                    
                }
            }
        }

        private void RetrieveFriction(Collision2D collision)
        {
            _material = collision.rigidbody.sharedMaterial;

            Friction = 0;

            if (_material != null)
            {
                Friction = _material.friction;
            }
        }
    }
}
