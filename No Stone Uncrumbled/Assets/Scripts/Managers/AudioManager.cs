using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer audioMixer;
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup soundEffectsMixerGroup;

    public string soundEffectsContainerName = "Sound Effects";

    public float fadeDuration = 2f;
    public AnimationCurve fadeCurve;

    public SoundEffect[] soundEffects;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        GameObject soundEffectsContainer = new GameObject(soundEffectsContainerName);
        soundEffectsContainer.transform.parent = transform;

        foreach (SoundEffect effect in soundEffects)
        {
            effect.source = soundEffectsContainer.AddComponent<AudioSource>();
            effect.source.outputAudioMixerGroup = soundEffectsMixerGroup;
        }
    }

    void Start()
    {
        AdjustMixerVolumes();
        audioMixer.SetFloat("MasterVolume", RemapVolume(0f));
    }

    public static float RemapVolume(float value01)
    {
        return Mathf.Lerp(-80f, 0f, value01);
    }

    public void AdjustMixerVolumes()
    {
        audioMixer.SetFloat("MasterVolume", RemapVolume(Settings.MasterVolume));
    }

    public void PlaySoundEffect(string name)
    {
        SoundEffect effect = Array.Find(soundEffects, s => s.name == name);

        if (effect == null)
        {
            Debug.LogWarning("Unable to find '" + name + "' sound effect!");
            return;
        }

        Clip randomClip = GetRandomClip(effect);

        effect.source.clip = randomClip.clip;
        effect.source.volume = randomClip.volume;
        effect.source.pitch = randomClip.pitch;

        effect.source.Play();
    }

    public static Clip GetRandomClip(SoundEffect effect)
    {
        if (effect.soundEffectClips.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, effect.soundEffectClips.Length);

            return effect.soundEffectClips[randomIndex];
        }

        return null;
    }

    public void Mute()
    {
        StartCoroutine(FadeOut());
    }

    public void Unmute()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float volume = fadeCurve.Evaluate(time / fadeDuration);

            audioMixer.SetFloat("MasterVolume", RemapVolume(Mathf.Clamp(volume, 0f, Settings.MasterVolume)));

            yield return 0;
        }
    }

    private IEnumerator FadeOut()
    {
        float time = fadeDuration;

        while (time > 0f)
        {
            time -= Time.unscaledDeltaTime;
            float volume = fadeCurve.Evaluate(time / fadeDuration);

            audioMixer.SetFloat("MasterVolume", RemapVolume(Mathf.Clamp(volume, 0f, Settings.MasterVolume)));

            yield return 0;
        }
    }
}
