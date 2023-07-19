using System;
using UnityEngine;

[Serializable]
public class Timer
{
    public event Action<float> TimerChanged;
    public event Action TimerFinished;
    public event Action<float> TimerReset;
    
    private float _maxTime;
    private float _remainingTime;

    public Timer(float maxTime)
    {
        ResetTimer(maxTime);
    }
    
    public void ResetTimer()
    {
        _remainingTime = _maxTime;
        
        TimerReset?.Invoke(_remainingTime);
    }
    public void ResetTimer(float maxTime)
    {
        _maxTime = maxTime;
        _remainingTime = _maxTime;
        
        TimerReset?.Invoke(_remainingTime);
    }
    public void SetMaxTime(float maxTime)
    {
        _maxTime = maxTime;
    }

    private void Update()
    {
        if (_remainingTime <= 0f) { return; }
        
        _remainingTime -= Time.deltaTime;
        TimerChanged?.Invoke(_remainingTime);
        
        if (_remainingTime <= 0f) { TimerFinished?.Invoke(); }
    }
}
