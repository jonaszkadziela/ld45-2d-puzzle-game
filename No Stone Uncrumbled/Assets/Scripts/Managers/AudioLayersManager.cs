using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using HelperMethods;

public class AudioLayersManager : MonoBehaviour
{
    public static AudioLayersManager Instance;

    public string audioLayersContainerName = "Audio Layers";
    public float fadeDuration = 2f;
    public AnimationCurve fadeCurve;
    public AudioLayer[] audioLayers;

    private bool initializedAudioLayers = false;

    void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        GameObject audioLayersContainer = new GameObject(audioLayersContainerName);
        audioLayersContainer.transform.parent = transform;

        foreach (AudioLayer layer in audioLayers)
        {
            layer.source = audioLayersContainer.AddComponent<AudioSource>();
            layer.source.outputAudioMixerGroup = AudioManager.Instance.musicMixerGroup;

            layer.source.loop = true;
            layer.source.mute = true;

            layer.source.clip = layer.audioLayerClip.clip;
            layer.source.volume = 0f;
            layer.source.pitch = layer.audioLayerClip.pitch;
            layer.source.Play();
        }

        UnmuteSceneLayer();
        initializedAudioLayers = true;
    }

    public void Reset()
    {
        for (int i = 0; i < audioLayers.Length; i++)
        {
            if (!audioLayers[i].source.mute)
            {
                StartCoroutine(FadeOut(audioLayers[i]));
            }
        }
    }

    public void Mute(string name)
    {
        AudioLayer layer = Array.Find(audioLayers, l => l.name == name);

        if (layer == null)
        {
            Debug.LogWarning("Unable to find '" + name + "' audio layer!");
            return;
        }

        StartCoroutine(FadeOut(layer));
    }

    public void Unmute(string name)
    {
        AudioLayer layer = Array.Find(audioLayers, l => l.name == name);

        if (layer == null)
        {
            Debug.LogWarning("Unable to find: " + name + " audio layer!");
            return;
        }

        StartCoroutine(FadeIn(layer));
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (initializedAudioLayers)
        {
            Reset();
            UnmuteSceneLayer(scene.name);
        }
    }

    private void UnmuteSceneLayer(string sceneName = "")
    {
        sceneName = sceneName == "" ? SceneManager.GetActiveScene().name : sceneName;

        switch (sceneName)
        {
            case "MainMenu":
                Unmute("MainMenu-Loop");
            break;

            case "Game":
                Unmute("Gameplay-Loop");
            break;
        }
    }

    private IEnumerator FadeIn(AudioLayer layer)
    {
        while (layer.isFading)
        {
            yield return new WaitForSeconds(0.1f);
        }

        layer.isFading = true;
        layer.source.mute = false;

        float time = 0f;
        float maxVolume = layer.audioLayerClip.volume;
        float duration = fadeDuration.Map(0f, fadeDuration, 0f, fadeDuration / maxVolume);

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            float volume = fadeCurve.Evaluate(time / duration);

            layer.source.volume = Mathf.Clamp(volume, 0f, maxVolume);

            yield return 0;
        }

        layer.isFading = false;
    }

    private IEnumerator FadeOut(AudioLayer layer)
    {
        while (layer.isFading)
        {
            yield return new WaitForSeconds(0.1f);
        }

        layer.isFading = true;

        float time = fadeDuration;
        float maxVolume = layer.audioLayerClip.volume;
        float duration = fadeDuration.Map(0f, fadeDuration, 0f, fadeDuration / maxVolume);

        while (time > 0f)
        {
            time -= Time.unscaledDeltaTime;
            float volume = fadeCurve.Evaluate(time / duration);

            layer.source.volume = Mathf.Clamp(volume, 0f, maxVolume);

            yield return 0;
        }

        layer.source.mute = true;
        layer.isFading = false;
    }
}
