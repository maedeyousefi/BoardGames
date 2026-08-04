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
    public PawnMover GetCurrentPawn() 
    { 
        PawnMover[] pawns = FindObjectsOfType<PawnMover>();
        foreach (PawnMover pawn in pawns) 
        {
            if (pawn.playerNumber == CurrentPlayerTurn)
                return pawn;
        } 
        return null; 
    }
    public int GetPlayerRank(int playerNumber)
    {
        return finishedPlayers.IndexOf(playerNumber) + 1;
    }
    public bool IsGameFinished()
    {
        if (PlayerCount == 2)
            return finishedPlayers.Count >= 1;

        if (PlayerCount == 3)
            return finishedPlayers.Count >= 2;

        if (PlayerCount == 4)
            return finishedPlayers.Count >= 3;

        return false;
    }
    public void FinishGame()
    {
        // اضافه کردن آخرین بازیکنی که هنوز تمام نکرده
        for (int i = 1; i <= PlayerCount; i++)
        {
            if (!finishedPlayers.Contains(i))
            {
                finishedPlayers.Add(i);
                break;
            }
        }

        Debug.Log("===== GAME OVER =====");

        for (int i = 0; i < finishedPlayers.Count; i++)
        {
            Debug.Log($"Rank {i + 1}: Player {finishedPlayers[i]}");
        }
        WinPanelManager.Instance.ShowWinPanel();
    }
    public void ResetGameData() 
    { 
        finishedPlayers.Clear();
        CurrentPlayerTurn = 1; 
       // consecutiveSixes = 0; 
    }
}