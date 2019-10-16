using UnityEngine;

public enum RockType
{
    Sandstone,
    Granite,
};

[RequireComponent(typeof(Rigidbody2D))]
public class Rock : InteractiveObject
{
    [Header("Statistics")]
    public RockType rockType = RockType.Sandstone;
    public float durability = 5f;
    [HideInInspector] public int number;
    [HideInInspector] public bool destroyed = false;

    [Header("Current state")]
    public bool pickedUp = false;

    private Rigidbody2D rb;
    private Transform slot;
    private AudioPlayer audioPlayer;
    private Vector3 previousPosition;

    private float distanceMoved = 0f;
    private int initialNumber;
    private bool pushed = false;
    private bool pushSoundEffectPlaying = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioPlayer = GetComponent<AudioPlayer>();

        initialNumber = Random.Range(
            GameSettings.RockInitialNumberRange.min,
            GameSettings.RockInitialNumberRange.max
        );

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
            if (Vector3.Distance(transform.position, slot.position) > GameSettings.MovementThreshold)
            {
                transform.position = slot.position;
            }
        }

        pushed = false;
        if (Vector3.Distance(transform.position, previousPosition) > GameSettings.MovementThreshold)
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
            DestroyRock();
        }
    }

    public void DestroyRock(bool delivered = false, float delay = 1f)
    {
        if (destroyed)
        {
            return;
        }

        string soundEffectName = delivered ? "Pop" : "Break";

        audioPlayer.StopSoundEffect("Push");
        audioPlayer.PlaySoundEffect(soundEffectName);

        Destroy(gameObject, delay);
        rb.constraints |= RigidbodyConstraints2D.FreezePosition;

        destroyed = true;
        pickedUp = false;
    }

    private void OnDestroy()
    {
        GameplayManager.Instance.rocksList.Remove(gameObject);
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
        if (destroyed)
        {
            return;
        }

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
