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

    public Transform[] boardCells;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        boardCells = new Transform[54]; 
        for (int i = 0; i < 54; i++) 
        {
            boardCells[i] = GameObject.Find($"Cell_{i:00}").transform;
        }
        Debug.Log($"Total Pawns: {allPawns.Count}");
        Debug.Log($"Current Turn: {GetCurrentPlayerColor()}");
        // تست ورود یک مهره
        // TryEnterPawn(allPawns[0], 6);
        //RollDice();
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
    public void TryEnterPawn(LudoPawn pawn, int diceValue)
    {
        if (!pawn.isInBase) return;
        if (diceValue != 6) return;
        int startCell = GetStartCell(pawn.pawnColor); 
        pawn.EnterBoard(startCell, boardCells[startCell]);
        Debug.Log($"{pawn.pawnColor} entered the board at {startCell}");
    }
    private int GetStartCell(PawnColor color)
    { switch (color) 
        { 
            case PawnColor.Purple: return 0;
            case PawnColor.Green: return 13;
            case PawnColor.Orange: return 27;
            case PawnColor.Blue: return 41;
        }
        return 0;
    }
    private LudoPawn GetFirstPawnInBase(PawnColor color) 
    { 
        foreach (var pawn in allPawns) 
        { 
            if (pawn.pawnColor == color && pawn.isInBase)
                return pawn;
        }
        return null;
    }
    public void RollDice()
    {
        int diceValue = Random.Range(1, 7);
        Debug.Log($"Dice: {diceValue}"); 
        PawnColor currentColor = GetCurrentPlayerColor();
        if (diceValue == 6)
        {
            LudoPawn pawn = GetFirstPawnInBase(currentColor); 
            if (pawn != null)
            {
                TryEnterPawn(pawn, 6);
                Debug.Log($"{currentColor} entered a pawn");
                // با 6 نوبت حفظ می‌شود
                 return;
            } 
        } // نوبت بعدی
           NextTurn();
    }
    public void DiceRolled(int diceValue)
    {
        Debug.Log($"Player {GetCurrentPlayerColor()} rolled {diceValue}");

        PawnColor color = GetCurrentPlayerColor();

        // اگر 6 آمد، یک مهره از خانه خارج شود
        if (diceValue == 6)
        {
            foreach (var pawn in allPawns)
            {
                if (pawn.pawnColor == color && pawn.isInBase)
                {
                    TryEnterPawn(pawn, diceValue);
                    return;
                }
            }
        }

        // فعلاً بعد از تاس نوبت عوض شود
        NextTurn();
    }
}