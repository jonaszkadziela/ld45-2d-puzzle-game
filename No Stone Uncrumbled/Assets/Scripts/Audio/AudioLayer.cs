using UnityEngine;

[System.Serializable]
public class AudioLayer
{
    public string name;
    [HideInInspector]
    public AudioSource source;
    public Clip audioLayerClip;
    [HideInInspector]
    public bool isFading = false;
}
