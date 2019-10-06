using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public static int Money;
    public static float MovementCredits;

    public static int SlotsLength;

    public static GameObject HeldObject;
    public static Transform[] Slots;

    public Transform slotsContainer;

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

    void Start()
    {
        Money = GameSettings.InitialMoney;
        MovementCredits = GameSettings.InitialMovementCredits;

        SlotsLength = slotsContainer.childCount;
        Slots = new Transform[SlotsLength];

        for (int i = 0; i < SlotsLength; i++)
        {
            Slots[i] = slotsContainer.GetChild(i);
        }
    }
}
