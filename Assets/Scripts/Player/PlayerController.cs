using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 5f;
    [Header("Rotation")] 
    [SerializeField] private float rotationSpeed = 20f;
    [Header("Shooting")] 
    [SerializeField] private Transform[] gunPoints;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float shootCooldown = 0.3f;
    [Header("Health")] 
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Collider2DInvoker[] hitBoxes;
    
    public int MaxHealth { get { return maxHealth; } set {} }
    public int CurrentHealth { get { return _currentHealth; } set {} }

    private Vector2 _targetVelocity;

    private int _currentGunPoint = 0;
    private float _remainingShootCooldown = 0f;

    private int _currentHealth;

    private Camera _cam;

    private void Start()
    {
        _currentHealth = maxHealth;
        _cam = Camera.main;
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

    private void Update()
    {
        HandleVelocity();
        HandleMouseRotation();
        HandleCooldowns();
        HandleShooting();
    }

    private void HandleVelocity()
    {
        _targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed;

        rb.velocity = Vector2.Lerp(rb.velocity, _targetVelocity, Time.deltaTime);
    }
    private void HandleMouseRotation()
    {
        var currentAngleDeg = transform.localEulerAngles.z;
        var worldPosition = GetMousePosAsWorld();
        var targetRotation = worldPosition - transform.position;
        targetRotation.Normalize();
        var targetAngleDeg = Mathf.Rad2Deg * Mathf.Atan2(targetRotation.y, targetRotation.x);

        // rb.rotation = Mathf.LerpAngle(currentAngleDeg, targetAngleDeg, Time.deltaTime * rotationSpeed);
        transform.localEulerAngles = new Vector3(0f, 0f, 
            Mathf.LerpAngle(currentAngleDeg, targetAngleDeg, Time.deltaTime * rotationSpeed));
    }
    private void HandleCooldowns()
    {
        if (_remainingShootCooldown > 0f) _remainingShootCooldown -= Time.deltaTime;
    }
    private void HandleShooting()
    {
        if (_remainingShootCooldown > 0f || !Input.GetMouseButton(0)) { return; }
        
        var shootVec = transform.right;
        shootVec.Normalize();

        var proj = Instantiate(projectilePrefab, gunPoints[_currentGunPoint].position, 
            Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(shootVec.y, shootVec.x)));
        proj.SetMoveVector(shootVec);

        IterateGunPoint();
        _remainingShootCooldown = shootCooldown;
    }

    private void IterateGunPoint()
    {
        _currentGunPoint++;
        if (_currentGunPoint >= gunPoints.Length)
        {
            _currentGunPoint -= gunPoints.Length;
        }
    }
    private Vector3 GetMousePosAsWorld()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = _cam.nearClipPlane;
        
        return _cam.ScreenToWorldPoint(mousePos);
    }

    public void CheckTrigger(Collider2D other)
    {
        if (!other.gameObject.TryGetComponent(out Projectile proj)) { return; }
        if (proj.DmgType != Projectile.DamagingType.Player) { return; }
        
        TakeDamage(proj.Damage);
        Destroy(proj.gameObject);
    }
    public void CheckCollision(Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out Projectile proj)) { return; }
        if (proj.DmgType != Projectile.DamagingType.Player) { return; }
        
        TakeDamage(proj.Damage);
        Destroy(proj.gameObject);
    }
    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        
        if (_currentHealth < 0) { GetDestroyed(); }
    }
    public void GetDestroyed()
    {
        Destroy(gameObject);
    }
}
