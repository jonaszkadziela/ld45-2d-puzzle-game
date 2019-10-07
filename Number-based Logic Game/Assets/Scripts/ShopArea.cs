using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ShopArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player)
        {
            AudioLayersManager.Instance.Mute("GameplayLoop");
            AudioLayersManager.Instance.Mute("GameplayLoopLowEnergy");
            AudioLayersManager.Instance.Unmute("ShopLoop");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player)
        {
            AudioLayersManager.Instance.Unmute("GameplayLoop");
            AudioLayersManager.Instance.Mute("ShopLoop");

            player.audioLayerEnabled = false;
        }
    }
}
