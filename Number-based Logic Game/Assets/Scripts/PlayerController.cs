using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public int money;
    public float energy;

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
        energy = GameSettings.InitialEnergy;
        money = GameSettings.InitialMoney;

        slotsLength = slotsContainer.childCount;
        slots = new Transform[slotsLength];

        for (int i = 0; i < slotsLength; i++)
        {
            slots[i] = slotsContainer.GetChild(i);
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

    public void DetermineCurrentEnergy(float distanceMoved)
    {
        energy = GameSettings.InitialEnergy - distanceMoved / GameSettings.EnergyDecreaseSlowness;

        if (energy <= 0)
        {
            GameManager.Instance.TriggerGameOver();
        }
    }
}
