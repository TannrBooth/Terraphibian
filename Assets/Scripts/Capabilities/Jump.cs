using UnityEngine;

namespace Terraphibian
{
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class Jump : MonoBehaviour
    {
        
        [SerializeField, Range(0f, 10f)] private float _jumpHeight = 3f;
        [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
        [SerializeField, Range(0f, 5f)] private float _downwardMovementMultiplier = 3f;
        [SerializeField, Range(0f, 5f)] private float _upwardMovementMultiplier = 1.7f;
        [SerializeField, Range(0f, 1f)] private float _jumpCutMultiplier = 0.5f;
        [SerializeField, Range(0f, 0.3f)] private float _coyoteTime = 0.2f;
        [SerializeField, Range(0f, 0.3f)] private float _jumpBuffer = 0.2f;

        public bool IsJumping { get; private set; }
        private Controller _controller;
        private Rigidbody2D _body;
        private CollisionDataRetriever _ground;
        private Vector2 _velocity;
        private WallInteractor _wallInteractor;

        private int _jumpPhase;
        private float _defaultGravityScale, _jumpSpeed, _coyoteCounter, _jumpBufferCounter;

        private bool _desiredJump, _onGround, _isWallJumping, _isJumpReset;

        void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _ground = GetComponent<CollisionDataRetriever>();
            _controller = GetComponent<Controller>();
            _wallInteractor = GetComponent<WallInteractor>();

            _isJumpReset = true;
            _defaultGravityScale = 1f;
        }

        void Update()
        {
            _desiredJump = _controller.input.RetrieveJumpInput(this.gameObject);
        }

        private void FixedUpdate()
        {
            _onGround = _ground.OnGround;
            _velocity = _body.linearVelocity;
            _isWallJumping = _wallInteractor.WallJumping;

            if (_isWallJumping)
            {
                _jumpPhase = 0;
            }
            
            if (_onGround && _body.linearVelocityY <= 0.01f)
            {
                _jumpPhase = 0;
                _coyoteCounter = _coyoteTime;
                IsJumping = false;
            }
            else
            {
                _coyoteCounter -= Time.deltaTime;
            }

            if (_desiredJump && _isJumpReset)
            {
                _isJumpReset = false;
                _desiredJump = false;
                _jumpBufferCounter = _jumpBuffer;
            }
            else if (_jumpBufferCounter > 0)
            {
                _jumpBufferCounter -= Time.deltaTime;
            }
            else if (!_desiredJump)
            {
                _isJumpReset = true;
            }

            if (_jumpBufferCounter > 0)
            {
                JumpAction();
            }

            if (_controller.input.RetrieveJumpInput(this.gameObject) && _body.linearVelocityY > 0)
            {
                _body.gravityScale = _upwardMovementMultiplier;
            }
            else if (!_controller.input.RetrieveJumpInput(this.gameObject) || _body.linearVelocityY < 0)
            {
                _body.gravityScale = _downwardMovementMultiplier;
            }
            else if (_body.linearVelocityY == 0)
            {
                _body.gravityScale = _defaultGravityScale;
            }
            if (!_controller.input.RetrieveJumpInput(this.gameObject) && _body.linearVelocityY > 0f && IsJumping)
            {
                _velocity.y *= _jumpCutMultiplier;
            }

            _body.linearVelocity = _velocity;


        }

        private void JumpAction()
        {
            if (_coyoteCounter > 0f || (_jumpPhase < maxAirJumps && (IsJumping || _isWallJumping)))
            {
                if (IsJumping || _isWallJumping)
                {
                    _jumpPhase += 1;
                }

                _jumpBufferCounter = 0;
                _coyoteCounter = 0;
                _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _jumpHeight * _upwardMovementMultiplier);
                IsJumping = true;

                if (_velocity.y > 0f)
                {
                    _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
                }
                _velocity.y = _jumpSpeed;
            }
        }
    }
}
