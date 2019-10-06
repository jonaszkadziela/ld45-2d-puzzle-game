using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Stone : InteractiveObject
{
    public enum PushDirections
    {
        TOP,
        RIGHT,
        BOTTOM,
        LEFT,
    };

    public bool isPickedUp = false;
    public bool isPushed = false;

    private Rigidbody2D rb;
    private int pushDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isPickedUp)
        {
            transform.position = PlayerController.Slots[pushDirection].position;
        }
    }

    public override void InteractionStart()
    {
        isPickedUp = true;
        pushDirection = DetermineDirection(PlayerController.Instance.transform.position);
    }

    public override void InteractionFinish()
    {
        isPickedUp = false;
    }

    private int DetermineDirection(Vector2 point)
    {
        if (Mathf.Abs(rb.position.x - point.x) > Mathf.Abs(rb.position.y - point.y))
        {
            if (rb.position.x < point.x)
            {
                return (int)PushDirections.LEFT;
            }
            else
            {
                return (int)PushDirections.RIGHT;
            }
        }
        else
        {
            if (rb.position.y < point.y)
            {
                return (int)PushDirections.BOTTOM;
            }
            else
            {
                return (int)PushDirections.TOP;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPushed = true;
            rb.constraints &= ~RigidbodyConstraints2D.FreezePosition;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPushed = false;
            rb.constraints |= RigidbodyConstraints2D.FreezePosition;
        }
    }
}
