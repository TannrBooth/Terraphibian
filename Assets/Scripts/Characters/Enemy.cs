using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected float _health = 3f;
    [SerializeField] protected float _speed = 1f;
    [SerializeField] protected int _attackPower = 1;
    [SerializeField] protected float _weight = 1f;

    protected virtual void Die()
    {
        Destroy(gameObject);
        // Generate drops
    }
}