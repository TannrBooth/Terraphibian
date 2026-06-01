using UnityEngine;
using System.Collections;

namespace Terraphibian
{
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class Sow : MonoBehaviour
    {
        [SerializeField] private int _sowCount = 10;
        [SerializeField, Range(0, 1)] private float _sowRange = 100;
        [SerializeField] private float _sowRecovery = 0.5f;
        [SerializeField] private float _sowDuration = 0.1f;
        [SerializeField] private float _sowSwitchDelay = 0.1f;
        [SerializeField] private float _sowRecoverCountdown = 0;
        [SerializeField] private float _sowSwitchCountdown = 0;
        [SerializeField] private float _sowBufferTime = 0.15f;

        [SerializeField] private float _throwHorizontalForce = 6f;
        [SerializeField] private float _throwVerticalForce = 10f;

        [SerializeField] private float _horizontalSpread = 2f;
        [SerializeField] private float _verticalSpread = 1f;

        [SerializeField] private Transform _throwPoint;

        [SerializeField] private GameObject _sowItemPrefab;
        
        private float _sowBufferCountdown;
        private bool _desiredSow;

        public bool IsSow { get; private set; }
        private Inventory _inventory;
        private CollisionDataRetriever _collisionDataRetriever;
        private Controller _controller;
        private Animator _animator;
        
        void Start()
        {
            _inventory = GetComponent<Inventory>();
            _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
            _controller = GetComponent<Controller>();
            if (GetComponent<Animator>()) _animator = GetComponent<Animator>();
        }

        void Update()
        {
            _desiredSow = _controller.input.RetrieveSowInputDown(this.gameObject);
            if (_desiredSow)
            {
                _sowBufferCountdown = _sowBufferTime;
            }
            else if (_sowBufferCountdown > 0)
            {
                _sowBufferCountdown -= Time.deltaTime;
            }

            if (_sowBufferCountdown > 0 && _sowRecoverCountdown <= 0)
            {
                if (_inventory == null) return;
                if (_inventory.GetSeedCount() < _sowCount) StartCoroutine(SowAction(_inventory.GetSeedCount()));
                else StartCoroutine(SowAction(_sowCount));
                _desiredSow = false;
            }

            if (_sowRecoverCountdown > 0)
            {
                _sowRecoverCountdown -= Time.deltaTime;
            }

            if (_sowSwitchCountdown > 0)
            {
                _sowSwitchCountdown -= Time.deltaTime;
            }
            else if (_sowSwitchCountdown <= 0 && _animator)
            {
                _animator.SetBool("firstSow", false);
            }
        }

        public IEnumerator SowAction(int count)
        {
            IsSow = true;
            _sowBufferCountdown = 0;
            _sowRecoverCountdown = _sowRecovery;
            float vertical = _controller.input.RetrieveUpDownInput(this.gameObject);
            if (Mathf.Abs(vertical) < 0.5)
            {
                // Sow in front of player
                Debug.Log("Sow in front of player");

                for (int i = 0; i < count; i++)
                {
                    GameObject collectable = Instantiate(
                        _sowItemPrefab,
                        _throwPoint.position,
                        Quaternion.identity
                    );

                    float facingDirection = transform.localScale.x > 0 ? 1f : -1f;

                    float randomX = Random.Range(-_horizontalSpread, _horizontalSpread);
                    float randomY = Random.Range(-_verticalSpread, _verticalSpread);

                    Vector2 force = new Vector2(
                        (facingDirection * _throwHorizontalForce) + randomX,
                        _throwVerticalForce + randomY
                    );
                    collectable.GetComponent<Collectable>().SetCollectDelay(.5f);

                    collectable.GetComponent<Collectable>().Launch(force);
                }
                _inventory.AddSeeds(-count);
            }
            else if (vertical > 0.5)
            {
                // Sow above player
                Debug.Log("Sow above player");
            }
            else if (vertical < -0.5 && !_collisionDataRetriever.OnGround)
            {
                // Sow below player
                Debug.Log("Sow below player");
            }

            if (_animator) 
            {
                _animator.SetTrigger("sowTrigger");
            }
            if (!_animator.GetBool("firstSow"))
            {
                _sowSwitchCountdown = _sowSwitchDelay;
            }

            // wait for recover
            yield return new WaitForSeconds(_sowDuration);
            if (_animator) 
            {
                _animator.SetBool("firstSow", true);
            }

            IsSow = false;
        }
    }
}