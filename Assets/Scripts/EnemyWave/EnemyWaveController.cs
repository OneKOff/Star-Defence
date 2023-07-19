using System;
using UnityEngine;

public class EnemyWaveController : MonoBehaviour
{
    private enum WaveState
    {
        None,
        Start,
        MidWave,
        End
    } 
    
    [SerializeField] private EnemyWaveInfo[] enemyWaves;
    [SerializeField] private EnemySpawnPoint[] spawnPoints;

    private int _currentWave;
    private int _currentEnemySpawn;
    private int _currentEnemyRepeat;
    private float _nextActionTimeLeftSeconds;
    private WaveState _waveState;

    private void Start()
    {
        _currentWave = 0;
        _currentEnemySpawn = 0;
        _currentEnemyRepeat = 0;
        _nextActionTimeLeftSeconds = enemyWaves[0].EnemySpawnInfos[0].StartDelaySeconds;
        _waveState = WaveState.Start;
    }

    private void Update()
    {
        _nextActionTimeLeftSeconds -= Time.deltaTime;

        if (_nextActionTimeLeftSeconds < 0f)
        {
            switch (_waveState)
            {
                case WaveState.Start:
                    _waveState = WaveState.MidWave;
                    break;
                case WaveState.MidWave:
                    IterateToNextRepeat();
                    break;
                case WaveState.End:
                    IterateToNextWave();
                    break;
                default:
                    break;
            }
        }
    }

    private void IterateToNextRepeat()
    {
        if (_currentEnemyRepeat + 1 >= enemyWaves[_currentWave].EnemySpawnInfos[_currentEnemySpawn].RepeatTimes)
        {
            _nextActionTimeLeftSeconds = enemyWaves[_currentWave].EnemySpawnInfos[_currentEnemySpawn].EndDelaySeconds;
            
            _currentEnemySpawn = 0;
            
            return;
        }

        Instantiate(enemyWaves[_currentWave].EnemyPrefabs[
                enemyWaves[_currentWave].EnemySpawnInfos[_currentEnemySpawn].EnemyIndexInList],
            spawnPoints[enemyWaves[_currentWave].EnemySpawnInfos[_currentEnemySpawn].EnemySpawnPointIndex].transform
                .position,
            Quaternion.identity);
        _currentEnemyRepeat++;
    }
    
    private void IterateToNextSpawn()
    {
        if (_currentEnemySpawn + 1 >= enemyWaves[_currentWave].EnemySpawnInfos.Length)
        {
            _waveState = WaveState.End;
            return;
        }
        
        _currentEnemySpawn++;
        _currentEnemyRepeat = 0;
    }

    private void IterateToNextWave()
    {
        if (_currentWave + 1 >= enemyWaves.Length)
        {
            _waveState = WaveState.None;
            return;
        }
        
        _currentWave++;
        _currentEnemySpawn = 0;
        _currentEnemyRepeat = 0;
        _waveState = WaveState.Start;
    }
}
