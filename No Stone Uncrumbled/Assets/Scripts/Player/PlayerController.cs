using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public int money;
    public float energy;
    public float initialEnergy;
    [HideInInspector]
    public float distanceMoved = 0f;

    [Range(0.01f, 1f)]
    public float lowEnergyThreshold = 0.2f;
    public bool audioLayerEnabled = false;
    public int slotsLength;

    public GameObject heldObject;
    public Transform[] slots;

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

        if (!audioLayerEnabled)
        {
            if (energy < GameSettings.InitialEnergy * lowEnergyThreshold)
            {
                AudioLayersManager.Instance.Unmute("GameplayLoopLowEnergy");
                audioLayerEnabled = true;
            }
        }
        else
        {
            if (energy >= GameSettings.InitialEnergy * lowEnergyThreshold)
            {
                AudioLayersManager.Instance.Mute("GameplayLoopLowEnergy");
                audioLayerEnabled = false;
            }
        }
    }

    public Transform DetermineNearestSlot(Vector3 objectPosition)
    {
        float minDistance = Mathf.Infinity;
        Transform nearestSlot = null;

        foreach (Transform t in slots)
        {
            float currentDistance = Vector3.Distance(t.position, objectPosition);

            if (currentDistance < minDistance)
            {
                minDistance = currentDistance;
                nearestSlot = t;
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
