using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public AnimationCurve animationCurve;

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

    void Update()
    {
        switch (PredefinedScene.GetScene(MainManager.ActiveSceneName))
        {
            case SceneEnum.Game:
                // TODO: Implement a proper Pause UI with quit button
                if (Input.GetButtonDown("Cancel"))
                {
                    QuitGame();
                }
            break;
        }
    }

    public void QuitGame()
    {
        SavesManager.Save();
        SceneFade.Instance.FadeOut();
        Invoke("ApplicationQuit", SceneFade.Instance.fadeDuration);
    }

    public void AnimatedCounter(TextMeshProUGUI numberValue, int targetNumber, float counterDelay = 0.1f)
    {
        StartCoroutine(CounterAnimation(numberValue, targetNumber, counterDelay));
    }

    private void ApplicationQuit()
    {
        Application.Quit();
    }

    private IEnumerator CounterAnimation(TextMeshProUGUI numberValue, int targetNumber, float counterDelay)
    {
        float time = 0f;
        float animationDuration = counterDelay * targetNumber;

        while (time < animationDuration)
        {
            time += Time.unscaledDeltaTime;

            float multiplier = animationCurve.Evaluate(time / animationDuration);
            int number = Mathf.FloorToInt(targetNumber * multiplier);

            numberValue.text = number.ToString();

            yield return 0;
        }
    }
}
