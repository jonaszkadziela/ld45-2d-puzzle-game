﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour
{
    public static SceneFade Instance;

    public Image overlay;
    public AnimationCurve fadeCurve;
    public float fadeDuration = 2f;

    private bool isFading = false;

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

    void Start()
    {
        FadeIn();
    }

    public void FadeIn()
    {
        if (isFading)
        {
            return;
        }

        AudioManager.Instance.Unmute();
        StartCoroutine(FadeInAnimation());
    }

    public void FadeOut()
    {
        if (isFading)
        {
            return;
        }

        AudioManager.Instance.Mute();
        StartCoroutine(FadeOutAnimation());
    }

    public void FadeTo(SceneEnum scene)
    {
        if (isFading)
        {
            return;
        }

        FadeOut();
        StartCoroutine(ChangeScene(scene));
    }

    private IEnumerator ChangeScene(SceneEnum scene)
    {
        while (isFading)
        {
            yield return 0;
        }

        SceneManager.LoadScene(PredefinedScene.GetName(scene));
    }

    private IEnumerator FadeInAnimation()
    {
        isFading = true;

        float time = fadeDuration;

        while (time > 0f)
        {
            time -= Time.unscaledDeltaTime;

            float alpha = fadeCurve.Evaluate(time / fadeDuration);
            overlay.color = new Color(0f, 0f, 0f, alpha);

            yield return 0;
        }

        isFading = false;
    }

    private IEnumerator FadeOutAnimation()
    {
        isFading = true;

        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;

            float alpha = fadeCurve.Evaluate(time / fadeDuration);
            overlay.color = new Color(0f, 0f, 0f, alpha);

            yield return 0;
        }

        isFading = false;
    }
}
