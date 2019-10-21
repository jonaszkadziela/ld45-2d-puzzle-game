using UnityEngine;

public static class SavesManager
{
    public static void Save()
    {
        /**
         * General Settings
         */

        PlayerPrefs.SetFloat("MasterVolume", Settings.MasterVolume);

        /**
         * Game Settings
         */

        PlayerPrefs.SetInt(
            "HighScore",
            Mathf.Max(GameplayManager.CurrentRound, GameplayManager.HighScore)
        );

        PlayerPrefs.Save();
    }

    public static void Load()
    {
        /**
         * General Settings
         */

        Settings.MasterVolume = PlayerPrefs.GetFloat("MasterVolume", Settings.InitialMasterVolume);

        /**
         * Game Settings
         */

        GameplayManager.HighScore = PlayerPrefs.GetInt("HighScore", GameSettings.InitialHighScore);
    }

    public static void Reset()
    {
        PlayerPrefs.DeleteAll();
        Load();
    }
}
