using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum DamagingType
    {
        None,
        Player,
        Building,
        Ally,
        AllFriendly,
        Enemy,
        EnemyBuilding,
        AllEnemies,
        Neutral,
        NeutralBuilding,
        AllNeutral,
        All
    } 
    
    [Header("General")]
    [SerializeField] private float lifetime = 2.0f;
    [Header("Movement")] 
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 10f;
    [Header("Hit Data")]
    [SerializeField] private int damage = 10;
    [SerializeField] private DamagingType dmgType;
    [SerializeField] private ParticleEffect impactParticlesPrefab;

    public int Damage => damage;
    public DamagingType DmgType => dmgType;
    
    private Vector2 _moveVector;

    private void Update()
    {
        HandleTimeLeft();
    }

    private void OnDestroy()
    {
        Instantiate(impactParticlesPrefab, transform.position, transform.rotation);
    }

    public void SetMoveVector(Vector2 moveVec)
    {
        _moveVector = moveVec;
        HandleMovement();
    }

    private void HandleTimeLeft()
    {
        lifetime -= Time.deltaTime;

        if (lifetime < 0f)
        {
            Destroy(gameObject);
        }
    }

    private void HandleMovement()
    {
        rb.velocity = _moveVector * speed;
    }
}
