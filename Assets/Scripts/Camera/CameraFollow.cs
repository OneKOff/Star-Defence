using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private Vector2 minBounds = new Vector2(-1f, -2f);
    [SerializeField] private Vector2 maxBounds = new Vector2(1f, 2f);
    [SerializeField] private bool isCameraMovementInverted = false;
    
    private Vector3 _pivotCameraPosition;

    private void Start()
    {
        _pivotCameraPosition = transform.position;
    }
    
    private void Update()
    {
        HandlePivotCalculation();
        HandleBoundaries();
        HandleMovement();
    }

    private void HandlePivotCalculation()
    {
        _pivotCameraPosition += (targetTransform.position - _pivotCameraPosition).normalized * cameraSpeed * Time.deltaTime;
    }
    
    private void HandleBoundaries()
    {
        var targetPos = targetTransform.position;

        if (_pivotCameraPosition.x - targetPos.x < minBounds.x)
        {
            _pivotCameraPosition = new Vector2(targetPos.x + minBounds.x, _pivotCameraPosition.y);
        }
        if (_pivotCameraPosition.x - targetPos.x > maxBounds.x)
        {
            _pivotCameraPosition = new Vector2(targetPos.x + maxBounds.x, _pivotCameraPosition.y);
        }
        if (_pivotCameraPosition.y - targetPos.y < minBounds.y)
        {
            _pivotCameraPosition = new Vector2(_pivotCameraPosition.x, targetPos.y + minBounds.y);
        }
        if (_pivotCameraPosition.y - targetPos.y > maxBounds.y)
        {
            _pivotCameraPosition = new Vector2(_pivotCameraPosition.x, targetPos.y + maxBounds.y);
        }
    }
    
    private void HandleMovement()
    {
        // If inverted, then camera will go toward player's movement. If not, camera chases player
        transform.position = isCameraMovementInverted ? 
            _pivotCameraPosition + 2 * (targetTransform.position - _pivotCameraPosition)
            : _pivotCameraPosition;
    }

    private void OnDrawGizmos()
    {
        var camPos = transform.position;
        
        Gizmos.color = Color.red;

        Gizmos.DrawLine(new Vector2(camPos.x + minBounds.x, camPos.y + minBounds.y), 
            new Vector2(camPos.x + minBounds.x,camPos.y +  maxBounds.y));
        Gizmos.DrawLine(new Vector2(camPos.x + minBounds.x, camPos.y + minBounds.y), 
            new Vector2(camPos.x + maxBounds.x, camPos.y + minBounds.y));
        Gizmos.DrawLine(new Vector2(camPos.x + maxBounds.x, camPos.y + minBounds.y), 
            new Vector2(camPos.x + maxBounds.x, camPos.y + maxBounds.y));
        Gizmos.DrawLine(new Vector2(camPos.x + minBounds.x, camPos.y + maxBounds.y), 
            new Vector2(camPos.x + maxBounds.x, camPos.y + maxBounds.y));
    }
}
