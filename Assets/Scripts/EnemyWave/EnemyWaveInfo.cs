using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveInfo : ScriptableObject
{
    [SerializeField] private Enemy[] enemyPrefabs;
    public Enemy[] EnemyPrefabs => enemyPrefabs;
    [SerializeField] private EnemySpawnInfo[] enemySpawnInfos;
    public EnemySpawnInfo[] EnemySpawnInfos => enemySpawnInfos;
}
