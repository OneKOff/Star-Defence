using System;
using System.Linq;
using UnityEngine;

public struct TimerPoolObject
{
    public Timer Timer;
    public bool IsUsed;
}

public class TimerController : MonoBehaviour
{
    public static TimerController Instance;

    [SerializeField] private int initialTimerAmount = 10;
    [SerializeField] private float defaultTime = 10f;

    private TimerPoolObject[] _availableTimers;
    
    private void Awake()
    {
        CheckInstance();
        SetTimers();
    }

    private void CheckInstance()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void SetTimers()
    {
        _availableTimers = new TimerPoolObject[initialTimerAmount];
        
        for (var i = 0; i < initialTimerAmount; i++)
        {
            SetNewTimer(i);
        }
    }
    private void SetNewTimer(int i)
    {
        var timer = new Timer(defaultTime);
            
        _availableTimers[i].Timer = timer;
        _availableTimers[i].IsUsed = false;
    }

    public Timer GetTimer(float maxTime)
    {
        var anyFree = false;
        var timerIndex = 0;
        
        for (var i = 0; i < _availableTimers.Length; i++)
        {
            if (_availableTimers[i].IsUsed) { continue; }

            timerIndex = i;
            anyFree = true;

            break;
        }

        if (!anyFree)
        {
            _availableTimers = _availableTimers.Concat(new TimerPoolObject[1]).ToArray();
            SetNewTimer(_availableTimers.Length);
        }

        _availableTimers[timerIndex].IsUsed = true;
        return _availableTimers[timerIndex].Timer;
    }

    public void ReturnTimer(Timer timer)
    {
        for (var i = 0; i < _availableTimers.Length; i++)
        {
            if (!_availableTimers[i].Timer.Equals(timer)) { continue; }

            _availableTimers[i].IsUsed = false;
            break;
        }
    }
}
