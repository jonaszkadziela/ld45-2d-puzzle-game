using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    [HideInInspector]
    public AudioSource source;
    public string name;
    [Range(0f, 1f)]
    public float pitchVariationRange = 0f;
    public bool loop = false;
    public Clip[] soundEffectClips;
}
