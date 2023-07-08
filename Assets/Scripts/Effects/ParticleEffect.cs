using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private float duration;

    private float _remainingDuration;
    
    private void Start()
    {
        ps.Play();
        _remainingDuration = duration;
    }
    
    private void Update()
    {
        _remainingDuration -= Time.deltaTime;
        
        if (_remainingDuration < 0f) { Destroy(gameObject); }
    }
}
