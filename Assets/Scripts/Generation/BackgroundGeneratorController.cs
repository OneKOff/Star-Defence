using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BackgroundGeneratorController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] backgroundTiles;
    [SerializeField] private Vector2Int mapSize = new Vector2Int(100, 100);
    
    private void Start()
    {
        for (var i = 0; i < mapSize.x; i++)
        {
            for (var j = 0; j < mapSize.y; j++)
            {
                var index = Random.Range(0, backgroundTiles.Length);
                Instantiate(backgroundTiles[index], 
                    (Vector2)transform.position + new Vector2((i - mapSize.x / 2) * backgroundTiles[index].size.x, 
                        (j - mapSize.y / 2) * backgroundTiles[index].size.y), Quaternion.identity, transform);
            }
        }
    }
}
