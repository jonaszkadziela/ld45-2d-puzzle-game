using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private GameObject nearbyObject = null;

    void Update()
    {
        if (!nearbyObject)
        {
            return;
        }

        if (Input.GetButtonDown("Interact"))
        {
            InteractiveObject interactiveObject = nearbyObject.GetComponent<InteractiveObject>();

            if (interactiveObject)
            {
                if (nearbyObject == PlayerController.HeldObject)
                {
                    PlayerController.HeldObject = null;

                    interactiveObject.InteractionFinish();
                }
                else
                {
                    if (!PlayerController.HeldObject)
                    {
                        PlayerController.HeldObject = nearbyObject;

                        interactiveObject.InteractionStart();
                    }
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!nearbyObject)
        {
            nearbyObject = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (nearbyObject == other.gameObject)
        {
            nearbyObject = null;
        }
    }
}
