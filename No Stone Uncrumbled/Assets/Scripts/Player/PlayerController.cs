using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public int money;
    public float energy;
    public float initialEnergy;
    [HideInInspector] public float distanceMoved = 0f;

    [Range(0.01f, 1f)] public float lowEnergyThreshold = 0.2f;
    public int slotsLength;
    public bool audioLayerEnabled = false;

    public Transform slotsContainer;
    public GameObject heldObject;
    [HideInInspector] public Transform[] slots;

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
        money = GameSettings.InitialMoney;
        energy = GameSettings.InitialEnergy;
        initialEnergy = GameSettings.InitialEnergy;        

        slotsLength = slotsContainer.childCount;
        slots = new Transform[slotsLength];

        for (int i = 0; i < slotsLength; i++)
        {
            slots[i] = slotsContainer.GetChild(i);
        }
    }

    void Update()
    {
        if (GameManager.GameOver)
        {
            return;
        }

        if (ShopArea.PlayingShopLayer)
        {
            audioLayerEnabled = false;
        }
        else
        {
            if (!audioLayerEnabled)
            {
                if (energy < GameSettings.InitialEnergy * lowEnergyThreshold)
                {
                    AudioLayersManager.Instance.Unmute("Gameplay-Loop-LowEnergy");
                    audioLayerEnabled = true;
                }
            }
            else
            {
                if (energy >= GameSettings.InitialEnergy * lowEnergyThreshold)
                {
                    AudioLayersManager.Instance.Mute("Gameplay-Loop-LowEnergy");
                    audioLayerEnabled = false;
                }
            }
        }
    }

    public Transform DetermineNearestSlot(Vector3 objectPosition)
    {
        float minDistance = Mathf.Infinity;
        Transform nearestSlot = null;

        foreach (Transform slot in slots)
        {
            float currentDistance = Vector3.Distance(slot.position, objectPosition);

            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                nearestSlot = slot;
            }
        }

        return nearestSlot;
    }

    public void DetermineCurrentEnergy()
    {
        energy = initialEnergy - distanceMoved / GameSettings.EnergyDecreaseSlowness;

        if (energy <= 0)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }
}
