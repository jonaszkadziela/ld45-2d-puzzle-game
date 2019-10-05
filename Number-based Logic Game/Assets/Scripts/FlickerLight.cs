using UnityEngine;

public class FlickerLight : MonoBehaviour
{
    public float flickerSpeed = 0.2f;
    public float multiplyScale = 1.05f;

    private Vector3 initialScale;
    private Vector3 targetScale;

    private bool increasing = true;

    void Start()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        targetScale = increasing ? initialScale * multiplyScale : initialScale;
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, flickerSpeed);

        if (Vector3.Distance(transform.localScale, targetScale) < 0.01f)
        {
            increasing = !increasing;
        }
    }
}
