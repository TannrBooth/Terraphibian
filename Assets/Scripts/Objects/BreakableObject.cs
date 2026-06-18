using UnityEngine;

namespace Terraphibian
{
    public class BreakableObject : MonoBehaviour
    {
        [SerializeField] protected int _hitsToBreak = 3;
        [SerializeField] protected GameObject _collectablePrefab;
        [SerializeField] protected int _collectableQuantity = 1;
        [SerializeField] private GameObject _hitEffectPrefab;

        private int _currentHits = 0;
        private int dropQuantity;

        public void Hit()
        {
            _currentHits++;
            if (_currentHits >= _hitsToBreak)
            {
                Break();
            }
            else
            {
                // Shake object and drop a few collectables on hit
                GetComponent<ShakeOnHit>().Shake();
                dropQuantity = UnityEngine.Random.Range(1, 3);
                DropCollectable(_collectablePrefab, dropQuantity);
                _collectableQuantity -= dropQuantity;
            }
            if (_hitEffectPrefab != null)
            {
                float randomZ = Random.Range(0f, 360f);

                GameObject effect = Instantiate(
                        _hitEffectPrefab,
                        transform.position,
                        Quaternion.Euler(0, 0, randomZ)
                );
                float randomScale = Random.Range(2.9f, 4.1f); // ±10%
                effect.transform.localScale *= randomScale;
            }
        }

        public void DropCollectable(GameObject collectablePrefab, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                GameObject collectable = Instantiate(collectablePrefab, transform.position, Quaternion.identity);
                Vector2 force = new Vector2(Random.Range(-4f, 4f),Random.Range(5f, 20f));

                collectable.GetComponent<Collectable>().Launch(force);
            }
        }

        protected virtual void Break()
        {
            DropCollectable(_collectablePrefab, _collectableQuantity);
            Destroy(gameObject);
            // Play break animation, spawn particles, etc.
        }
    }
}
