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

        if (Input.GetButtonDown("Interact"))
        {
            if (interactiveObject.gameObject == PlayerController.Instance.heldObject)
            {
                PlayerController.Instance.heldObject = null;

                interactiveObject.InteractionFinish();
            }
            else
            {
                if (!PlayerController.Instance.heldObject)
                {
                    PlayerController.Instance.heldObject = interactiveObject.gameObject;

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
