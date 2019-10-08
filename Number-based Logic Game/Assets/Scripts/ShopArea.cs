using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ShopArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player)
        {
            AudioLayersManager.Instance.Reset();
            AudioLayersManager.Instance.Unmute("ShopLoop");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player)
        {
            CancelInvoke();

            AudioLayersManager.Instance.Reset();
            AudioLayersManager.Instance.Unmute("GameplayLoop");

            player.audioLayerEnabled = false;
        }
    }
}
