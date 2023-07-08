using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private bool useAcceleration = false;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 3f;
    [SerializeField] private float maxSpeed = 5f;
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Collider2DInvoker[] hitBoxes;

    private int _currentHealth;
    private Transform _currentTarget;

    private void Start()
    {
        _currentHealth = maxHealth;
        _currentTarget = GameController.Instance.PlayerObject.transform;
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
        var velocity = rb.velocity;
        var movementVector = (_currentTarget.position - transform.position).normalized;
        if (useAcceleration)
        {
            var xAccel = Math.Abs(Mathf.Sign(velocity.x) - Mathf.Sign(movementVector.x)) < Mathf.Epsilon
                ? acceleration * movementVector.x
                : deceleration * movementVector.x;
            var yAccel = Math.Abs(Mathf.Sign(velocity.y) - Mathf.Sign(movementVector.y)) < Mathf.Epsilon
                ? acceleration * movementVector.y
                : deceleration * movementVector.y;

            var accelerationVector = new Vector2(xAccel, yAccel);
            velocity += accelerationVector * Time.deltaTime;
        }
        else
        {
            velocity = movementVector * acceleration;
        }

        velocity = ClampVelocityManually(velocity);
        rb.velocity = velocity;

        Debug.Log("Velocity magnitude: " + rb.velocity.magnitude);
        transform.localEulerAngles = new Vector3(0, 0, 
            Mathf.Rad2Deg * Mathf.Atan2(movementVector.y, movementVector.x));
    }

    private Vector2 ClampVelocityManually(Vector2 vector)
    {
        if (vector.magnitude > maxSpeed)
        {
            vector = vector.normalized * maxSpeed;
        }

        return vector;
    }

    public void CheckTrigger(Collider2D other)
    {
        if (!other.gameObject.TryGetComponent(out Projectile proj)) { return; }

        if (proj.DmgType != Projectile.DamagingType.Enemy) { return; }
        
        TakeDamage(proj.Damage);
        Destroy(proj.gameObject);
    }
    public void CheckCollision(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out Projectile proj)) { return; }

        if (proj.DmgType != Projectile.DamagingType.Enemy) { return; }
        
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
}
