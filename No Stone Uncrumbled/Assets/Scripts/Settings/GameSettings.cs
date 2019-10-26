public static class GameSettings
{
    /**
     * Player Settings
     */

    public const int InitialMoney = 0;
    public const float InitialEnergy = 50f;

    public static int MoneyRewardDecreasePerMinute = 10;
    public static float EnergyDecreaseSlowness = 10f;

    public static RangeInt MoneyRewardRange = new RangeInt(5, 20);
    public static RangeInt EnergyRewardRange = new RangeInt(5, 20);

    /**
     * Gameplay Settings
     */

    public const int InitialCurrentRound = 0;
    public const int InitialCurrentNumber = 0;
    public const int InitialHighScore = 0;

    public static float MovementThreshold = 0.01f;

    public static RangeInt RockSpawnAmountRange = new RangeInt(20, 40);
    public static RangeInt RockInitialNumberRange = new RangeInt(5, 30);
    public static int RockSpawnMoreThreshold = 10;
    public static float GranitePercentage = 0.2f;

    public static RangeInt TargetNumberRange = new RangeInt(50, 150);
    public static RangeInt TargetMarginRange = new RangeInt(10, 20);
    public static RangeInt MinTargetMarginRange = new RangeInt(1, 5);

    public static int TargetNumberIncrease = 20;
    public static int TargetMarginDecrease = 2;
}
