using UnityEngine;

public class GameSettings : MonoBehaviour
{
    /**
     * Public variables to set values in UnityEditor
     */

    [Header("Player Settings")]
    public int initialMoney = 0;
    public float initialEnergy = 100f;

    [Header("Gameplay Settings")]
    public RangeInt stoneSpawnAmount = new RangeInt(20, 40);
    public RangeInt stoneInitialNumberRange = new RangeInt(10, 30);
    public int stoneSpawnMoreThreshold = 10;

    public RangeInt targetNumber = new RangeInt(50, 150);
    public RangeInt targetMargin = new RangeInt(10, 20);
    public RangeInt minTargetMargin = new RangeInt(1, 5);

    public int targetNumberIncrease = 20;
    public int targetMarginDecrease = 2;

    /**
     * Public static variables to use them easily in other scripts
     */
    
    public static int InitialMoney;
    public static float InitialEnergy;

    public static RangeInt StoneSpawnAmount;
    public static RangeInt StoneInitialNumberRange;
    public static int StoneSpawnMoreThreshold;

    public static RangeInt TargetNumber;
    public static RangeInt TargetMargin;
    public static RangeInt MinTargetMargin;

    public static int TargetNumberIncrease;
    public static int TargetMarginDecrease;

    void Awake()
    {
        InitialMoney = initialMoney;
        InitialEnergy = initialEnergy;

        StoneSpawnAmount = stoneSpawnAmount;
        StoneInitialNumberRange = stoneInitialNumberRange;
        StoneSpawnMoreThreshold = stoneSpawnMoreThreshold;

        TargetNumber = targetNumber;
        TargetMargin = targetMargin;
        MinTargetMargin = minTargetMargin;

        TargetNumberIncrease = targetNumberIncrease;
        TargetMarginDecrease = targetMarginDecrease;
    }
}
