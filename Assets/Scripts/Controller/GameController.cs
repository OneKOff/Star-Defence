using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private void InitializeInstance()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    public PlayerController PlayerObject { get; private set; }
    
    private void Awake()
    {
        InitializeInstance();
        var player = FindObjectOfType<PlayerController>();
        SetPlayerObject(player);
    }

    public void SetPlayerObject(PlayerController player)
    {
        PlayerObject = player;
    }
}
