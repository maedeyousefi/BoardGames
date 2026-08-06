using System.Collections.Generic;
using UnityEngine;

public class LudoGameManager : MonoBehaviour
{
    public static LudoGameManager Instance;

    [Header("Players")]
    public int playerCount = 4;
    public int currentPlayer = 0;

    [Header("Players Pawns")]
    public List<LudoPawn> allPawns = new List<LudoPawn>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start() 
    {
        Debug.Log($"Total Pawns: {allPawns.Count}");
        Debug.Log($"Current Turn: {GetCurrentPlayerColor()}");
        NextTurn(); NextTurn(); NextTurn();
    }
    public PawnColor GetCurrentPlayerColor() 
    { 
        switch (currentPlayer) 
        { 
            case 0: return PawnColor.Blue;
            case 1: return PawnColor.Orange; 
            case 2: return PawnColor.Green; 
            case 3: return PawnColor.Purple;
            default: return PawnColor.Blue;
        } 
    }
    public void NextTurn() 
    { 
        currentPlayer++; 
        if (currentPlayer >= playerCount) 
            currentPlayer = 0; 
        Debug.Log($"Current Turn: {GetCurrentPlayerColor()}");
    }
}