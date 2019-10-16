using UnityEngine;

[System.Serializable]
public class AudioLayer
{
    public string name;
    public Clip audioLayerClip;
    [HideInInspector] public AudioSource source;
    [HideInInspector] public bool isFading = false;
}
