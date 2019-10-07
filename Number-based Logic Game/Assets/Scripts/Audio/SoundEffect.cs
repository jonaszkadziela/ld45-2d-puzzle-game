using UnityEngine;

[System.Serializable]
public class SoundEffect
{
    public string name;
    [HideInInspector]
    public AudioSource source;
    public Clip[] soundEffectClips;
}
