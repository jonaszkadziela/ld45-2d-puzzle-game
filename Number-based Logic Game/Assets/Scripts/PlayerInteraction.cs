using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private InteractiveObject interactiveObject = null;

    void Update()
    {
        if (!interactiveObject)
        {
            return;
        }

        if (Input.GetButtonDown("Interact") && interactiveObject)
        {
            if (interactiveObject.gameObject == PlayerController.HeldObject)
            {
                PlayerController.HeldObject = null;

                interactiveObject.InteractionFinish();
            }
            else
            {
                if (!PlayerController.HeldObject)
                {
                    PlayerController.HeldObject = interactiveObject.gameObject;

                    interactiveObject.InteractionStart();
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!interactiveObject)
        {
            interactiveObject = other.GetComponent<InteractiveObject>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (interactiveObject)
        {
            if (other.gameObject == interactiveObject.gameObject)
            {
                interactiveObject = null;
            }
        }
    }
}
