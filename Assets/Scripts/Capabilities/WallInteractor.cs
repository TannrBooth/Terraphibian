using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Terraphibian
{ 
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class WallInteractor : MonoBehaviour
    {
        public bool WallJumping { get; private set; }

        [Header("Wall Slide")]
        [SerializeField][Range(0.1f, 5f)] private float _wallSlideMaxSpeed = 2f;
        [Header("Wall Jump")]
        [SerializeField] private Vector2 _wallJumpClimb = new Vector2(4f, 12f);
        [SerializeField] private Vector2 _wallJumpBounce = new Vector2(10.7f, 10f);
        [SerializeField] private Vector2 _wallJumpLeap = new Vector2(14f, 12f);
        [SerializeField, Range(0.05f, 0.5f)] private float _wallStickTime = .25f;

        private CollisionDataRetriever _collisionDataRetriever;
        private Rigidbody2D _body;
        private Controller _controller;

        private Vector2 _velocity;
        private bool _onWall, _onGround, _desiredJump, _isJumpReset;
        private float _wallDirectionX, _wallStickCounter;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _body = GetComponent<Rigidbody2D>();
            _controller = GetComponent<Controller>();

            _isJumpReset = true;
        }

        // Update is called once per frame
        void Update()
        {

            _desiredJump = _controller.input.RetrieveJumpInput(this.gameObject);

        }

        void FixedUpdate()
        {
            _velocity = _body.linearVelocity;
            _onWall = _collisionDataRetriever.OnWall;
            _onGround = _collisionDataRetriever.OnGround;
            _wallDirectionX = _collisionDataRetriever.ContactNormal.x;

            #region Wall Slide
            if (_onWall)
            {
                if (_velocity.y < -_wallSlideMaxSpeed)
                {
                    _velocity.y = -_wallSlideMaxSpeed;
                }
                if (!_collisionDataRetriever.OnGround)
                transform.localScale = new Vector2(_wallDirectionX, 1);
            }

            #region Wall Stick
            if(_collisionDataRetriever.OnWall && !_collisionDataRetriever.OnGround && !WallJumping)
            {
                if (_wallStickCounter > 0)
                {
                    _velocity.x = 0;

                    if (_controller.input.RetrieveMoveInput(this.gameObject) != 0 &&
                        Mathf.Sign(_controller.input.RetrieveMoveInput(this.gameObject)) == Math.Sign(_collisionDataRetriever.ContactNormal.x))
                    {
                        _wallStickCounter -= Time.deltaTime;
                    }
                    else
                    {
                        _wallStickCounter = _wallStickTime;
                    }
                }
                else
                {
                    _wallStickCounter = _wallStickTime;
                }
            }
            #endregion

            #endregion

            #region Wall Jump

            if ((_onWall && _velocity.x == 0) || _onGround)
            {
                WallJumping = false;
            }

            if (_onWall && !_onGround)
            {
                if (_desiredJump && _isJumpReset)
                {
                    if (_controller.input.RetrieveMoveInput(this.gameObject) == 0) // Bounce
                    {
                        _velocity = new Vector2(_wallJumpBounce.x * _wallDirectionX, _wallJumpBounce.y);
                        WallJumping = true;
                        _desiredJump = false;
                        _isJumpReset = false;
                    }
                    else if (Mathf.Sign(-_wallDirectionX) == Mathf.Sign(_controller.input.RetrieveMoveInput(this.gameObject))) // Climb
                    {
                        _velocity = new Vector2(_wallJumpClimb.x * _wallDirectionX, _wallJumpClimb.y);
                        WallJumping = true;
                        _desiredJump = false;
                        _isJumpReset = false;
                    }
                    else // Leap
                    {
                        _velocity = new Vector2(_wallJumpLeap.x * _wallDirectionX, _wallJumpLeap.y);
                        WallJumping = true;
                        _desiredJump = false;
                        _isJumpReset = false;
                    }
                }

                else if (!_desiredJump)
                {
                    _isJumpReset = true;
                }
            }
            #endregion

            _body.linearVelocity = _velocity;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _collisionDataRetriever.EvaluateCollision(collision);
            _isJumpReset = false;

            if(_collisionDataRetriever.OnWall && !_collisionDataRetriever.OnGround && WallJumping)
            {
                _body.linearVelocity = Vector2.zero;
            }
        }
    }
}
