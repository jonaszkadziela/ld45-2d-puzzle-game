using UnityEngine;

public enum GameState
{
    NumericPuzzle,
    CollectingNumbers,
    GameOver,
};

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static GameState State;

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

        State = GameState.CollectingNumbers;
    }

    public void TriggerGameOver()
    {
        State = GameState.GameOver;
    }
}
