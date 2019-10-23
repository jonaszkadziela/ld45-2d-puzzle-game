using System;

public enum SceneEnum
{
    MainMenu,
    Game,
}

[System.Serializable]
public class PredefinedScene
{
    public string name;
    public SceneEnum scene;

    public static string GetName(SceneEnum scene)
    {
        PredefinedScene predefinedScene = Array.Find(MainManager.Instance.scenes, s => s.scene == scene);

        if (predefinedScene == null)
        {
            throw new NullReferenceException("Unable to find '" + scene + "' scene!");
        }

        return predefinedScene.name;
    }

    public static SceneEnum GetScene(string name)
    {
        PredefinedScene predefinedScene = Array.Find(MainManager.Instance.scenes, s => s.name == name);

        if (predefinedScene == null)
        {
            throw new NullReferenceException("Unable to find '" + name + "' scene!");
        }

        return predefinedScene.scene;
    }
}
