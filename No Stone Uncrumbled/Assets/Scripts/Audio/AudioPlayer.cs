using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public SoundEffect[] soundEffects;

    void Start()
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();

        foreach (SoundEffect effect in soundEffects)
        {
            effect.source = audioSource;
        }
    }

    public void PlaySoundEffect(string name)
    {
        SoundEffect effect = System.Array.Find(soundEffects, s => s.name == name);

        if (effect == null)
        {
            Debug.LogWarning("Unable to find '" + name + "' sound effect!");
            return;
        }

        Clip randomClip = AudioManager.GetRandomClip(effect);
        float pitch = randomClip.pitch + Random.Range(-effect.pitchVariationRange, effect.pitchVariationRange);

        effect.source.clip = randomClip.clip;
        effect.source.volume = randomClip.volume;
        effect.source.pitch = pitch;
        effect.source.loop = effect.loop;

        effect.source.Play();
    }

    public void StopSoundEffect(string name)
    {
        SoundEffect effect = System.Array.Find(soundEffects, s => s.name == name);

        if (effect == null)
        {
            Debug.LogWarning("Unable to find '" + name + "' sound effect!");
            return;
        }

        effect.source.Stop();
    }
}
