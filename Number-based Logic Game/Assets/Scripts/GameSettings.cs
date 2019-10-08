﻿using UnityEngine;

public class GameSettings : MonoBehaviour
{
    /**
     * Public variables to set values in UnityEditor
     */

    [Header("Player Settings")]
    public int initialMoney = 0;
    public int moneyRewardDecreasePerMinute = 10;
    public float initialEnergy = 50f;
    public float energyDecreaseSlowness = 10f;
    public RangeInt moneyRewardRange = new RangeInt(10, 50);
    public RangeInt energyRewardRange = new RangeInt(10, 50);

    [Header("Gameplay Settings")]
    public RangeInt stoneSpawnAmount = new RangeInt(20, 40);
    public RangeInt stoneInitialNumberRange = new RangeInt(5, 30);
    public int stoneSpawnMoreThreshold = 10;
    public float boulderPercentage = 0.2f;

    public RangeInt targetNumberRange = new RangeInt(50, 150);
    public RangeInt targetMarginRange = new RangeInt(10, 20);
    public RangeInt minTargetMarginRange = new RangeInt(1, 5);

    public int targetNumberIncrease = 20;
    public int targetMarginDecrease = 2;

    /**
     * Public static variables to use them easily in other scripts
     */
    
    public static int InitialMoney;
    public static int MoneyRewardDecreasePerMinute;
    public static float InitialEnergy;
    public static float EnergyDecreaseSlowness;
    public static RangeInt MoneyRewardRange;
    public static RangeInt EnergyRewardRange;

    public static RangeInt StoneSpawnAmount;
    public static RangeInt StoneInitialNumberRange;
    public static int StoneSpawnMoreThreshold;
    public static float BoulderPercentage;

    public static RangeInt TargetNumberRange;
    public static RangeInt TargetMarginRange;
    public static RangeInt MinTargetMarginRange;

    public static int TargetNumberIncrease;
    public static int TargetMarginDecrease;

    void Awake()
    {
        InitialMoney = initialMoney;
        MoneyRewardDecreasePerMinute = moneyRewardDecreasePerMinute;
        InitialEnergy = initialEnergy;
        MoneyRewardRange = moneyRewardRange;
        EnergyRewardRange = energyRewardRange;
        EnergyDecreaseSlowness = energyDecreaseSlowness;

        StoneSpawnAmount = stoneSpawnAmount;
        StoneInitialNumberRange = stoneInitialNumberRange;
        StoneSpawnMoreThreshold = stoneSpawnMoreThreshold;
        BoulderPercentage = boulderPercentage;

        TargetNumberRange = targetNumberRange;
        TargetMarginRange = targetMarginRange;
        MinTargetMarginRange = minTargetMarginRange;

        TargetNumberIncrease = targetNumberIncrease;
        TargetMarginDecrease = targetMarginDecrease;
    }
}
