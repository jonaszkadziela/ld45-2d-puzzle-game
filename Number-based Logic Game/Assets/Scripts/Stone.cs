using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Stone : InteractiveObject
{
    [Header("Statistics")]
    [HideInInspector]
    public int initialNumber;
    [HideInInspector]
    public int number;
    public float durability = 10f;

    [Header("Current state")]
    public bool isPickedUp = false;
    [HideInInspector]
    public bool isPushed = false;

    private Rigidbody2D rb;
    private Transform slot;
    private Vector3 previousPosition;
    private float distanceMoved = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        number = initialNumber;
        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (number > 0)
        {
            if (isPickedUp && slot)
            {
                if (Vector3.Distance(transform.position, slot.position) > 0.01f)
                {
                    transform.position = slot.position;
                }
            }

            isPushed = false;
            if (Vector3.Distance(transform.position, previousPosition) > 0.01f)
            {
                distanceMoved += Vector3.Distance(transform.position, previousPosition);
                DetermineCurrentNumber(distanceMoved);

                isPushed = true;
                previousPosition = transform.position;
            }
        }
    }

    private void DetermineCurrentNumber(float distanceMoved)
    {
        number = initialNumber - (int)(distanceMoved / durability);

        if (number <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // TODO: Show particles
        GameplayManager.Instance.stonesList.Remove(gameObject);
    }

    public override void InteractionStart()
    {
        isPickedUp = true;
        slot = PlayerController.Instance.DetermineNearestSlot(transform.position);
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
