﻿using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    [Header("Player Settings")]
    public int initialMoney = 0;
    public float initialEnergy = 100f;

    [Header("Gameplay Settings")]
    public RangeInt stoneSpawnAmount = new RangeInt(20, 40);
    public RangeInt stoneInitialNumberRange = new RangeInt(10, 30);
    public int stoneSpawnMoreThreshold = 10;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
