using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private Vector2 minBounds = new Vector2(-0.5f, -1.5f);
    [SerializeField] private Vector2 maxBounds = new Vector2(0.5f, 1.5f);

    private void Update()
    {
        transform.Translate((targetTransform.position - transform.position).normalized * cameraSpeed);
    }

    private void HandleBoundaries()
    {
        var transform1 = transform;
        
        if (transform1.position.x - targetTransform.position.x < minBounds.x)
        {
            transform1.position = new Vector2(targetTransform.position.x + minBounds.x + 0.1f, transform1.position.y);
        }
        if (transform1.position.x - targetTransform.position.x > maxBounds.x)
        {
            transform1.position = new Vector2(targetTransform.position.x + maxBounds.x - 0.1f, transform1.position.y);
        }
        if (transform1.position.y - targetTransform.position.y < minBounds.y)
        {
            transform1.position = new Vector2(transform1.position.x, targetTransform.position.y + minBounds.y + 0.1f);
        }
        if (transform1.position.y - targetTransform.position.y > maxBounds.y)
        {
            transform1.position = new Vector2(transform1.position.x, targetTransform.position.y + maxBounds.y - 0.1f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var targetPosition = targetTransform.position;
        Gizmos.DrawLine(new Vector3(targetPosition.x + minBounds.x, targetPosition.y + minBounds.y, -5), 
            new Vector3(targetPosition.x + minBounds.x,targetPosition.y +  maxBounds.y, -5));
        Gizmos.DrawLine(new Vector3(targetPosition.x + minBounds.x, targetPosition.y + minBounds.y, -5), 
            new Vector3(targetPosition.x + maxBounds.x, targetPosition.y + minBounds.y, -5));
        Gizmos.DrawLine(new Vector3(targetPosition.x + maxBounds.x, targetPosition.y + minBounds.y, -5), 
            new Vector3(targetPosition.x + maxBounds.x, targetPosition.y + maxBounds.y, -5));
        Gizmos.DrawLine(new Vector3(targetPosition.x + minBounds.x, targetPosition.y + maxBounds.y, -5), 
            new Vector3(targetPosition.x + maxBounds.x, targetPosition.y + maxBounds.y, -5));
    }
}
