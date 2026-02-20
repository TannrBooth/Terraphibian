using UnityEngine;
namespace Terraphibian
{
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class Move : MonoBehaviour
    {
        [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
        [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
        [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;
        [SerializeField, Range(0f, 100f)] private float _pushDecay = 10f;
        [SerializeField, Range(0.05f, 1f)] public float _pushbackWeight = .5f;

        private Controller _controller;
        private Vector2 _desiredVelocity, _velocity, _pushback;
        public Vector2 _direction;
        private Rigidbody2D _body;
        private CollisionDataRetriever _collisionDataRetriever;
        private Melee _melee;

        private float _maxSpeedChange, _acceleration;
        private bool _onGround;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _controller = GetComponent<Controller>();
            _melee = GetComponent<Melee>();
        }

        public void ApplyPushback(Vector2 dir, float force)
            {
                _pushback += dir.normalized * force;
            }

        // Update is called once per frame
        void Update()
        {
            _direction.x = _controller.input.RetrieveMoveInput(this.gameObject);
            _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max(maxSpeed - _collisionDataRetriever.Friction, 0f);
            if (_desiredVelocity.x < -0.01 && !_collisionDataRetriever.OnWall && !_melee.IsMelee)
                transform.localScale = new Vector2(-1, 1);
            else if (_desiredVelocity.x > 0.01 && !_collisionDataRetriever.OnWall && !_melee.IsMelee)
                transform.localScale = new Vector2(1, 1);
        }

        private void FixedUpdate()
        {
            _onGround = _collisionDataRetriever.OnGround;
            _velocity = _body.linearVelocity;

            _acceleration = _onGround ? maxAcceleration : maxAirAcceleration;
            _maxSpeedChange = _acceleration * Time.deltaTime;
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

            _velocity += _pushback; 
            _body.linearVelocity = _velocity;

            _pushback = Vector2.Lerp(_pushback, Vector2.zero, _pushDecay * Time.fixedDeltaTime);
        }
    }
}
