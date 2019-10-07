using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance;

    [Header("Player Settings")]
    public int initialMoney = 0;
    public float initialEnergy = 100f;

    [Header("Gameplay Settings")]
    public RangeInt stoneSpawnRate;
    public RangeInt stoneNumberRange;

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
