using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class NumberOperationArea : MonoBehaviour
{
    public NumberOperation operation;

    private Collider2D col;

    void Start()
    {
        col = GetComponent<Collider2D>();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (col.bounds.Contains(other.bounds.min) && col.bounds.Contains(other.bounds.max))
        {
            Stone stone = other.GetComponent<Stone>();

            if (stone && !stone.destroyed)
            {
                stone.DestroyStone(true);
                GameplayManager.Instance.ChangeCurrentNumber(operation, stone.number);
            }
        }
    }
}
