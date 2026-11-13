using UnityEngine;
namespace Terraphibian
{
public class Enemy : MonoBehaviour
    {
        [SerializeField] public float _health = 3f;
        [SerializeField] protected float _speed = 1f;
        [SerializeField] protected int _attackPower = 1;
        [SerializeField] protected float _weight = 1f;

        protected virtual void Die()
        {
            CameraShake.Instance.Shake(0.5f, 0.5f);
            Destroy(gameObject);
            // Generate drops
        }
    }
}