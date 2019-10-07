using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ShopArea : MonoBehaviour
{
    private Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (col.bounds.Contains(other.bounds.min) && col.bounds.Contains(other.bounds.max))
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player)
            {
                // TODO: Play shop music
            }
        }
    }
}
