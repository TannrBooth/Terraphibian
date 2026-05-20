using System.Collections;
using UnityEngine;

namespace Terraphibian
{
    
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class Melee : MonoBehaviour
    {
        [SerializeField] private float _attackDuration = 0.1f;
        [SerializeField] private float _attackRecovery = .25f;
        [SerializeField] private float _attackSwitchDelay = 0.1f;
        [SerializeField] private AttackBoxController _attackBoxController;
        [SerializeField] public float _pushbackMultiplier = 1.5f;
        [SerializeField] public float _meleeBufferTime = 0.15f;

        public bool IsMelee { get; private set; }
        private CollisionDataRetriever _collisionDataRetriever;
        private SpriteRenderer _spriteRenderer;
        private bool _desiredMelee;
        private float _attackRecoverCountdown = 0;
        private float _attackSwitchCountdown = 0;
        private float _meleeBufferCountdown;
        private Controller _controller;
        private Animator _animator;

        private GameObject _attackBoxInstance;

        void Start()
        {
            _controller = GetComponent<Controller>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            if (GetComponent<Animator>()) _animator = GetComponent<Animator>();

            _attackBoxInstance = transform.Find("AttackBox").gameObject;
            if (!_attackBoxInstance)
            {
                Debug.LogError("Attack box instance not found!");
            }
        }

        void Update()
        {
            _desiredMelee = _controller.input.RetrieveMeleeInputDown(this.gameObject);
            if (_desiredMelee)
            {
                _meleeBufferCountdown = _meleeBufferTime;
            }
            else if (_meleeBufferCountdown > 0)
            {
                _meleeBufferCountdown -= Time.deltaTime;
            }

            if (_meleeBufferCountdown > 0 && _attackRecoverCountdown <= 0)
            {
                StartCoroutine(MeleeAction());
                _desiredMelee = false;
            }

            if (_attackRecoverCountdown > 0)
            {
                _attackRecoverCountdown -= Time.deltaTime;
            }

            if (_attackSwitchCountdown > 0)
            {
                _attackSwitchCountdown -= Time.deltaTime;
            }
            else if (_attackSwitchCountdown <= 0 && _animator)
            {
                _animator.SetBool("firstSwing", false);
                _attackBoxInstance.GetComponent<Animator>().SetBool("firstSwing", false);
            }
        }

        public IEnumerator MeleeAction()
        {
            IsMelee = true;
            _meleeBufferCountdown = 0;
            _attackRecoverCountdown = _attackRecovery;
            float vertical = _controller.input.RetrieveUpDownInput(this.gameObject);
            if (Mathf.Abs(vertical) < 0.5)
            {
                _attackBoxController.ApplyConfig(AttackDirection.Neutral);
            }
            else if (vertical > 0.5)
            {
                _attackBoxController.ApplyConfig(AttackDirection.Up);
            }
            else if (vertical < -0.5 && !_collisionDataRetriever.OnGround)
            {
                _attackBoxController.ApplyConfig(AttackDirection.Down);
            }

            if (_animator) _animator.SetBool("isMelee", true);
            if (!_animator.GetBool("firstSwing"))
            {
                _attackSwitchCountdown = _attackSwitchDelay;
            }

            _attackBoxInstance.GetComponent<Animator>().SetTrigger("melee1");
            
            _attackBoxInstance.GetComponent<PolygonCollider2D>().enabled = true;

            // wait for recover
            yield return new WaitForSeconds(_attackDuration);
            if (_animator) 
            {
                _animator.SetBool("firstSwing", true);
                _attackBoxInstance.GetComponent<Animator>().SetBool("firstSwing", true);
            }
            _attackBoxInstance.GetComponent<PolygonCollider2D>().enabled = false;
            _attackBoxController.ResetTransform();
            _attackBoxController._pushbackApplied = false;
            IsMelee = false;
            if (_animator) _animator.SetBool("isMelee", false);
        }
    }
}
