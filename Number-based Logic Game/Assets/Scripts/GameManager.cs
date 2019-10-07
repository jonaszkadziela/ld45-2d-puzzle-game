using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static bool GameOver = false;

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

    public void TriggerGameOver()
    {
        GameOver = true;
    }
}
