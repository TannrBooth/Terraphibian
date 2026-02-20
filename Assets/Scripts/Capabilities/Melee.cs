using System.Collections;
using UnityEngine;

namespace Terraphibian
{
    
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class Melee : MonoBehaviour
    {
        [SerializeField] private float _attackDuration = 0.1f;
        [SerializeField] private float _attackRecovery = .25f;
        [SerializeField] private AttackBoxController _attackBoxController;
        [SerializeField] public float _pushbackMultiplier = 1.5f;

        public bool IsMelee { get; private set; }
        private CollisionDataRetriever _collisionDataRetriever;
        private SpriteRenderer _spriteRenderer;
        private bool _desiredMelee;
        private float _attackRecoverCountdown = 0;
        private Controller _controller;
        public Sprite meleeSprite;
        public Sprite idleSprite;

        void Start()
        {
            _controller = GetComponent<Controller>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            meleeSprite = Resources.Load<Sprite>("Graphics/Character_Sprites/Terraphibian_slash");
            idleSprite = Resources.Load<Sprite>("Graphics/Character_Sprites/Terraphibian");
        }

        void Update()
        {
            _desiredMelee = _controller.input.RetrieveMeleeInputDown(this.gameObject);

            if (_desiredMelee && _attackRecoverCountdown <= 0)
            {
                StartCoroutine(MeleeAction());
                _desiredMelee = false;
            }

            if (_attackRecoverCountdown > 0)
            {
                _attackRecoverCountdown -= Time.deltaTime;
            }
        }

        public IEnumerator MeleeAction()
        {
            IsMelee = true;
            _attackRecoverCountdown = _attackRecovery;
            float vertical = _controller.input.RetrieveUpDownInput(this.gameObject);
            if (Mathf.Abs(vertical) < 0.5)
            {
                _spriteRenderer.sprite = meleeSprite;
                _spriteRenderer.color = Color.red;
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

            // wait for recover
            yield return new WaitForSeconds(_attackDuration);

            _attackBoxController.ResetTransform();
            _attackBoxController._pushbackApplied = false;
            _spriteRenderer.color = Color.white;
            _spriteRenderer.sprite = idleSprite;
            IsMelee = false;
        }
    }
}
