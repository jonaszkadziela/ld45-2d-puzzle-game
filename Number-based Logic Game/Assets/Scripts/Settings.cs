using UnityEngine;

public class Settings : MonoBehaviour
{
    /**
     * Public variables to set values in UnityEditor
     */

    [Header("Audio Settings")]
    public float masterVolume = 1f;

    /**
     * Public static variables to use them easily in other scripts
     */

    public static float MasterVolume;

    void Awake()
    {
        MasterVolume = masterVolume;
    }
}
