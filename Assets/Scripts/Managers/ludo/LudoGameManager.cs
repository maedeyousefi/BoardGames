using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LudoGameManager : MonoBehaviour
{
    public enum TurnState
    {
        DiceReady,
        WaitingPawn,
        Moving
    }

    public TurnState turnState = TurnState.DiceReady;
    public static LudoGameManager Instance;
    public LudoPawn selectedPawn;
    public LudoDice dice;

    public int currentDiceValue;
    [Header("Players")]
    public int playerCount = 4;
    public int currentPlayer = 0;
    public bool extraTurn = false;
    public bool waitingForPawn = false;
    public bool waitingForMove = false;

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
        dice.EnableRoll();
        turnState = TurnState.DiceReady;
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
        //pawn.EnterBoard(startCell, boardCells[startCell]);
        StartCoroutine(pawn.MoveSteps(diceValue, boardCells));
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
    //public void RollDice()
    //{
    //  int diceValue = Random.Range(1, 7);
    //Debug.Log($"Dice: {diceValue}"); 
    //PawnColor currentColor = GetCurrentPlayerColor();
    //   if (diceValue == 6)
    //  {
    //     LudoPawn pawn = GetFirstPawnInBase(currentColor); 
    //     if (pawn != null)
    //   {
    //     TryEnterPawn(pawn, 6);
    //   Debug.Log($"{currentColor} entered a pawn");
    // با 6 نوبت حفظ می‌شود
    //  return;
    // } 
    // } // نوبت بعدی
    //  NextTurn();
    //  }
    public void DiceRolled(int diceValue)
    {
        currentDiceValue = diceValue;
        Debug.Log($"{GetCurrentPlayerColor()} rolled {diceValue}");

        PawnColor currentColor = GetCurrentPlayerColor();
        bool canEnter = diceValue == 6 && GetFirstPawnInBase(currentColor) != null;
        bool canMove = HasMovablePawnOnBoard(currentColor);

        if (!canEnter && !canMove)
        {
            Debug.Log("حرکتی ممکن نیست، نوبت بعدی");
            currentDiceValue = 0;
            turnState = TurnState.DiceReady;
            NextTurn();
            dice.EnableRoll();
            return;
        }

        turnState = TurnState.WaitingPawn;
        dice.DisableRoll();          // تا مهره انتخاب نشه، تاس قفله (عمدی)
        Debug.Log("منتظر انتخاب مهره...");
    }
    private bool HasMovablePawnOnBoard(PawnColor color)
    {
        foreach (var pawn in allPawns)
        {
            if (pawn.pawnColor == color && !pawn.isInBase && !pawn.hasFinished)
                return true;
        }
        return false;
    }
    public void SelectPawn(LudoPawn pawn)
    {
        if (turnState != TurnState.WaitingPawn)
        {
            Debug.Log("الان نمیشه مهره انتخاب کرد");
            return;
        }

        if (pawn.pawnColor != GetCurrentPlayerColor())
        {
            Debug.Log("این مهره نوبت شما نیست");
            return;
        }

        selectedPawn = pawn;

        // ورود مهره با 6
        if (pawn.isInBase && currentDiceValue == 6)
        {
            int start = GetStartCell(pawn.pawnColor);
            pawn.EnterBoard(start, boardCells[start]);
            currentDiceValue = 0;
            turnState = TurnState.DiceReady;
            dice.EnableRoll();       // تاس جایزه
            return;
        }

        // حرکت مهره‌ای که روی صفحه‌ست
        if (!pawn.isInBase && currentDiceValue > 0)
        {
            int move = currentDiceValue;
            bool wasSix = (move == 6);
            currentDiceValue = 0;
            turnState = TurnState.Moving;
            StartCoroutine(MoveAndFinish(pawn, move, wasSix));
        }
    }

    private IEnumerator MoveAndFinish(LudoPawn pawn, int steps, bool wasSix)
    {
        yield return StartCoroutine(pawn.MoveSteps(steps, boardCells));

        turnState = TurnState.DiceReady;
        if (wasSix)
        {
            dice.EnableRoll();       // جایزه دوباره، نوبت عوض نمیشه
        }
        else
        {
            NextTurn();
            dice.EnableRoll();
        }
    }
}