[System.Serializable]
public struct EnemySpawnInfo
{
    public int EnemyIndexInList;
    public int EnemySpawnPointIndex;
    
    public float InnerDelaySeconds;
    public float StartDelaySeconds;
    public float EndDelaySeconds;
    
    public int RepeatTimes;
}