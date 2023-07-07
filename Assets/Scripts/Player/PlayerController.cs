using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 5f;
    [Header("Rotation")] 
    [SerializeField] private float rotationSpeed = 180f;
    [Header("Shooting")] 
    [SerializeField] private Transform[] gunPoints;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float shootCooldown = 0.5f;

    private Vector2 _targetVelocity;
    private Vector2 _targetRotation;
    private float _remainingCooldown = 0f;

    private Ray ray;
    private RaycastHit hitInfo;
    
    
    private void Update()
    {
        HandleVelocity();
        HandleRotation();
        HandleCooldowns();
        if (_remainingCooldown <= 0f && Input.GetMouseButtonDown(0))
        {
            HandleShooting();
        }
    }

    private void HandleVelocity()
    {
        _targetVelocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed;

        rb.velocity = Vector2.Lerp(rb.velocity, _targetVelocity, Time.deltaTime);
    }

    private void HandleRotation()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            _targetRotation = transform.position - hitInfo.point;
            _targetRotation.Normalize();

            transform.localEulerAngles = new Vector3(0f, 0f, 
                Mathf.Atan2(_targetRotation.y, _targetRotation.x));
        }
    }

    private void HandleCooldowns()
    {
        if (_remainingCooldown > 0f) _remainingCooldown -= Time.deltaTime;
    }
    
    private void HandleShooting()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo))
        {
            var shootVec =  transform.position - hitInfo.point;
            shootVec.Normalize();

            var proj = Instantiate(projectilePrefab, gunPoints[Random.Range(0, gunPoints.Length - 1)].position, 
                Quaternion.Euler(0, 0, Mathf.Atan2(shootVec.y, shootVec.x)), transform);
            
            proj.SetMoveVector(shootVec);
        }

        _remainingCooldown = shootCooldown;
    }
}
