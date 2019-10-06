using UnityEngine;

public class InteractiveObject : MonoBehaviour
{
    public virtual void InteractionStart()
    {
        Debug.Log("Interaction started!");
    }

    public virtual void InteractionFinish()
    {
        Debug.Log("Interaction finished!");
    }
}
