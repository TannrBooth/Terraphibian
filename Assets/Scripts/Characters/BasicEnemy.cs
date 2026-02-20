using Terraphibian;
using UnityEngine;

public class BasicEnemy : Enemy
{
    private Move _move;
    private Jump _jump;
    private WallInteractor _wallInteractor;
    private CollisionDataRetriever _collisionDataRetriever;

    void Awake()
    {
        _move = GetComponent<Move>();
        _jump = GetComponent<Jump>();
        _wallInteractor = GetComponent<WallInteractor>();
        _collisionDataRetriever = GetComponent<CollisionDataRetriever>();
    }

    void Start()
    {
        _health = 10;
    }

    void Update()
    {
        if (_health <= 0)
        {
            Die();
            Debug.Log(gameObject.name + " died");
        }
    }

    void FixedUpdate()
    {

    }
}