using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public static string ActiveSceneName;

    public PredefinedScene[] scenes;

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

        SceneManager.sceneLoaded += OnSceneLoaded;
        SavesManager.Load();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ActiveSceneName = SceneManager.GetActiveScene().name;

        StartCoroutine(UnmuteSceneLayer(scene));
    }

    private IEnumerator UnmuteSceneLayer(Scene scene)
    {
        while (!AudioLayersManager.InitializedAudioLayers)
        {
            yield return new WaitForSeconds(0.1f);
        }

        AudioLayersManager.Instance.Reset();

        switch (PredefinedScene.GetScene(scene.name))
        {
            case SceneEnum.MainMenu:
                AudioLayersManager.Instance.Unmute("MainMenu-Loop");
            break;

            case SceneEnum.Game:
                AudioLayersManager.Instance.Unmute("Gameplay-Loop");
            break;
        }
    }
}
