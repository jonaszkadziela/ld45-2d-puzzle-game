using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void Play()
    {
        SceneFade.Instance.FadeTo(SceneEnum.Game);
    }
}
