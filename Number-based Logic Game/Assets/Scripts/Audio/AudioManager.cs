using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer audioMixer;
    public AudioMixerGroup masterGroup;

    public float fadeDuration = 1f;
    public AnimationCurve fadeCurve;

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
    }

    void Start()
    {
        AdjustMixerVolumes();
        audioMixer.SetFloat("MasterVolume", RemapVolume(0f));

        Unmute();
    }

    public static float RemapVolume(float value01)
    {
        return Mathf.Lerp(-80f, 0f, value01);
    }

    public void AdjustMixerVolumes()
    {
        audioMixer.SetFloat("MasterVolume", RemapVolume(Settings.MasterVolume));
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
