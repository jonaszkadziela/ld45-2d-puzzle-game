using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public string name;
    public bool loop = false;
    [Range(0f, 1f)] public float pitchVariationRange = 0f;
    public Clip[] soundEffectClips;
    [HideInInspector] public AudioSource source;
}
