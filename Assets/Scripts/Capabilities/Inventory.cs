using UnityEngine;

namespace Terraphibian
{
    [RequireComponent(typeof(Controller), typeof(CollisionDataRetriever), typeof(Rigidbody2D))]
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private int _maxHp = 5;
        [SerializeField] private int _seedCount = 0;
        [SerializeField] private int _maxSeedCount = 100;

        private int _currentHp;


        void Awake()
        {
            _currentHp = _maxHp;
        }

        void Update()
        {
            
        }

        void FixedUpdate()
        {
            
        }

        public void AddSeeds(int amount)
        {
            _seedCount = Mathf.Clamp(_seedCount + amount, 0, _maxSeedCount);
        }
        public int GetSeedCount()
        {
            return _seedCount;
        }
    }
    
}
