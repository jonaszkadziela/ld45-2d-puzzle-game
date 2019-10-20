using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ShopArea : MonoBehaviour
{
    public static bool PlayingShopLayer;

    private float cooldown;
    private bool insideShop = false;

    void Start()
    {
        PlayingShopLayer = false;
    }

    void Update()
    {
        if (GameManager.GameOver)
        {
            return;
        }

        cooldown -= Time.unscaledDeltaTime;

        if (cooldown < 0f)
        {
            if (insideShop)
            {
                if (!PlayingShopLayer)
                {
                    AudioLayersManager.Instance.Mute("Gameplay-Loop");
                    AudioLayersManager.Instance.Mute("Gameplay-Loop-LowEnergy");
                    AudioLayersManager.Instance.Unmute("Shop-Loop");

                    PlayingShopLayer = true;
                }
            }
            else
            {
                if (PlayingShopLayer)
                {
                    AudioLayersManager.Instance.Mute("Shop-Loop");
                    AudioLayersManager.Instance.Unmute("Gameplay-Loop");

                    PlayingShopLayer = false;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player)
        {
            cooldown = AudioLayersManager.Instance.fadeDuration / 2f;
            insideShop = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player)
        {
            cooldown = AudioLayersManager.Instance.fadeDuration / 2f;
            insideShop = false;
        }
    }
}
