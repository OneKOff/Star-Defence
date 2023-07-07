using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float lifetime = 2.0f;
    [Header("Movement")] 
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 10f;
    
    private Vector2 _moveVector;

    private void Update()
    {
        HandleTimeLeft();
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
