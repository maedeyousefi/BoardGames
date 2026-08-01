using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameType SelectedGame;

    public int PlayerCount = 2;
    public int CurrentPlayerTurn = 1;
    public List<int> finishedPlayers = new List<int>();
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
        do
        {
            CurrentPlayerTurn++;

            if (CurrentPlayerTurn > PlayerCount)
            {
                CurrentPlayerTurn = 1;
            }

        } while (finishedPlayers.Contains(CurrentPlayerTurn));

        Debug.Log("نوبت بازیکن: " + CurrentPlayerTurn);
    }
    public void RegisterFinishedPlayer(int playerNumber)
    {
        if (!finishedPlayers.Contains(playerNumber))
        {
            finishedPlayers.Add(playerNumber);

            Debug.Log("Player " + playerNumber + " Finished!");
        }
    }
}