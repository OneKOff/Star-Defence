using System;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool useAcceleration = false;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 3f;
    [SerializeField] private float driftAcceleration = 10f;
    [SerializeField] private float maxSpeed = 5f;
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Collider2DInvoker[] hitBoxes;
    [Header("Attacking")] 
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private float shootCooldown = 1f;
    [SerializeField] private float attackAggroRange = 3f;

    public int MaxHealth { get { return maxHealth; } set {} }
    public int CurrentHealth { get { return _currentHealth; } set {} }
    
    private int _currentHealth;
    private Transform _currentTarget;

    private float _currentShootCD;

    private void Start()
    {
        _currentHealth = maxHealth;
        _currentTarget = GameController.Instance.PlayerObject.transform;

        _currentShootCD = shootCooldown;
    }
    
    private void OnEnable()
    {
        for (var i = 0; i < hitBoxes.Length; i++)
        {
            hitBoxes[i].TriggerEnter2D += CheckTrigger;
            hitBoxes[i].CollisionEnter2D += CheckCollision;
        }
    }

    private void OnDisable()
    {
        for (var i = 0; i < hitBoxes.Length; i++)
        {
            hitBoxes[i].TriggerEnter2D -= CheckTrigger;
            hitBoxes[i].CollisionEnter2D -= CheckCollision;
        }
    }

    public void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleCooldowns();
        HandleAttacking();
    }

    private void HandleMovement()
    {
        var velocity = rb.velocity;
        var movementVector = (_currentTarget.position - transform.position).normalized;
        if (useAcceleration)
        {
            var xDiff = Math.Abs(Mathf.Sign(velocity.x) - Mathf.Sign(movementVector.x)) > Mathf.Epsilon;
            var yDiff = Math.Abs(Mathf.Sign(velocity.y) - Mathf.Sign(movementVector.y)) > Mathf.Epsilon;
            
            var xAccel = xDiff
                ? yDiff 
                    ? deceleration * movementVector.x 
                    : driftAcceleration * movementVector.x
                : acceleration * movementVector.x;
            var yAccel = yDiff
                ? xDiff 
                    ? deceleration * movementVector.y 
                    : driftAcceleration * movementVector.y
                : acceleration * movementVector.y;
            // var xAccel = mlt * movementVector.x;
            // var yAccel = mlt * movementVector.y;

            var accelerationVector = new Vector2(xAccel, yAccel);
            velocity += accelerationVector * Time.deltaTime;
        }
        else
        {
            velocity = movementVector * acceleration;
        }
        
        velocity = ClampVelocity(velocity);
        rb.velocity = velocity;
    }
    private void HandleRotation()
    {
        var movementVector = (_currentTarget.position - transform.position).normalized;
        
        transform.localEulerAngles = new Vector3(0, 0, 
            Mathf.Rad2Deg * Mathf.Atan2(movementVector.y, movementVector.x));
    }
    private void HandleCooldowns()
    {
        if (_currentShootCD <= 0f) { return; }

        _currentShootCD -= Time.deltaTime;
    }
    private void HandleAttacking()
    {
        if (_currentShootCD > 0f || (transform.position - _currentTarget.position).magnitude > attackAggroRange) { return; }
        
        var shootVec = transform.right;
        shootVec.Normalize();

        var proj = Instantiate(projectilePrefab, gunPoint.position, 
            Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(shootVec.y, shootVec.x)));
        proj.SetMoveVector(shootVec);

        _currentShootCD = shootCooldown;
    }

    private Vector2 ClampVelocity(Vector2 vector)
    {
        if (vector.magnitude > maxSpeed)
        {
            vector = vector.normalized * maxSpeed;
        }

        return vector;
    }

    #region IDamageable Scripts
    public void CheckTrigger(Collider2D other)
    {
        if (!other.gameObject.TryGetComponent(out Projectile proj)) { return; }
        if (proj.DmgType != Projectile.DamagingType.Enemy) { return; }
        
        Debug.Log($"Enemy triggered by {other.gameObject.name}");
        
        TakeDamage(proj.Damage);
        Destroy(proj.gameObject);
    }
    public void CheckCollision(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out Projectile proj)) { return; }
        if (proj.DmgType != Projectile.DamagingType.Enemy) { return; }
        
        Debug.Log($"Enemy collided with {other.gameObject.name}");
        
        TakeDamage(proj.Damage);
        Destroy(proj.gameObject);
    }
    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        if (_currentHealth <= 0) { GetDestroyed(); }
    }
    public void GetDestroyed()
    {
        Destroy(gameObject);
    }
    #endregion
}
