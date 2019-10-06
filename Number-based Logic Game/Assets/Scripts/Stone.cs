using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Stone : InteractiveObject
{
    public bool isPickedUp = false;
    public bool isPushed = false;

    private Rigidbody2D rb;
    private Transform slot;
    private Vector3 previousPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (isPickedUp && slot)
        {
            if (Vector3.Distance(transform.position, slot.position) > 0.01f)
            {
                transform.position = slot.position;
            }
        }

        isPushed = false;
        if (previousPosition != transform.position)
        {
            isPushed = true;
            previousPosition = transform.position;
        }
    }

    public override void InteractionStart()
    {
        isPickedUp = true;
        slot = PlayerController.DetermineNearestSlot(transform.position);
    }

    public override void InteractionFinish()
    {
        isPickedUp = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rb.constraints &= ~RigidbodyConstraints2D.FreezePosition;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            rb.constraints |= RigidbodyConstraints2D.FreezePosition;
        }
    }
}
