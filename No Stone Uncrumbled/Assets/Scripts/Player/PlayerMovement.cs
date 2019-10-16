using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Animator animator;

    public float initialSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector3 previousPosition;

    private float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = initialSpeed;
        previousPosition = transform.position;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        if (GameManager.GameOver)
        {
            return;
        }

        speed = initialSpeed;

        if (PlayerController.Instance.heldObject)
        {
            Rock rock = PlayerController.Instance.heldObject.GetComponent<Rock>();

            if (rock)
            {
                float rockMass = rock.GetComponent<Rigidbody2D>().mass / 10f;
                float massFactor = rockMass / (rb.mass + rockMass);

                speed *= 1 - massFactor;
            }
        }

        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, previousPosition) > GameSettings.MovementThreshold)
        {
            PlayerController.Instance.distanceMoved += Vector3.Distance(transform.position, previousPosition);
            PlayerController.Instance.DetermineCurrentEnergy();

            previousPosition = transform.position;
        }
    }
}
