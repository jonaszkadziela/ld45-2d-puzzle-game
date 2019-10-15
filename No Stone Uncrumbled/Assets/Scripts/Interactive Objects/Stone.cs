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
    public bool regularStone = true;
    [HideInInspector]
    public bool destroyed = false;

    [Header("Current state")]
    public bool pickedUp = false;
    [HideInInspector]
    public bool pushed = false;

    private Rigidbody2D rb;
    private AudioPlayer audioPlayer;

    private Transform slot;
    private Vector3 previousPosition;
    private float distanceMoved = 0f;
    private bool pushSoundEffectPlaying = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioPlayer = GetComponent<AudioPlayer>();

        number = initialNumber;
        previousPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (destroyed)
        {
            return;
        }

        if (pickedUp && slot)
        {
            if (Vector3.Distance(transform.position, slot.position) > 0.01f)
            {
                transform.position = slot.position;
            }
        }

        pushed = false;
        if (Vector3.Distance(transform.position, previousPosition) > 0.01f)
        {
            distanceMoved += Vector3.Distance(transform.position, previousPosition);
            DetermineCurrentNumber(distanceMoved);

            pushed = true;
            previousPosition = transform.position;
        }

        if (!pushSoundEffectPlaying)
        {
            if (pushed)
            {
                audioPlayer.PlaySoundEffect("Push");
                pushSoundEffectPlaying = true;
            }
        }
        else
        {
            if (!pushed)
            {
                audioPlayer.StopSoundEffect("Push");
                pushSoundEffectPlaying = false;
            }
        }
    }

    private void DetermineCurrentNumber(float distanceMoved)
    {
        number = initialNumber - (int)(distanceMoved / durability);

        if (number <= 0)
        {
            DestroyStone();
        }
    }

    public void DestroyStone(bool delivered = false,  float time = 1f)
    {
        if (destroyed)
        {
            return;
        }

        string soundEffectName = delivered ? "Pop" : (regularStone ? "StoneBreak" : "BoulderBreak");

        audioPlayer.StopSoundEffect("Push");
        audioPlayer.PlaySoundEffect(soundEffectName);

        Destroy(gameObject, time);
        rb.constraints |= RigidbodyConstraints2D.FreezePosition;

        destroyed = true;
        pickedUp = false;
    }

    private void OnDestroy()
    {
        GameplayManager.Instance.stonesList.Remove(gameObject);
    }

    public override void InteractionStart()
    {
        if (destroyed)
        {
            return;
        }

        pickedUp = true;
        slot = PlayerController.Instance.DetermineNearestSlot(transform.position);
    }

    public override void InteractionFinish()
    {
        pickedUp = false;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && !destroyed)
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
