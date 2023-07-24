using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldUIFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0f, 1f, 0f);
    
    private Transform _followTarget;
    
    private void Update()
    {
        if (!_followTarget) { return; }
        
        transform.position = _followTarget.position + offset;
    }

    public void SetTarget(Transform fTarget)
    {
        _followTarget = fTarget;
    }
}
