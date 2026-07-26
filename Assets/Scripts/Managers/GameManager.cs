using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameType SelectedGame;

    public int PlayerCount = 2;
    public int CurrentPlayerTurn = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void NextTurn()
    {
        CurrentPlayerTurn++;
        if (CurrentPlayerTurn > PlayerCount)
        {
            CurrentPlayerTurn = 1;
        }

        Debug.Log("نوبت بازیکن: " + CurrentPlayerTurn);
    }
}