using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LudoGameManager : MonoBehaviour
{
    public enum TurnState
    {
        DiceReady,
        WaitingPawn,
        Moving
    }
    [Header("Final Paths")]
    public Transform[] blueFinalPath;
    public Transform[] orangeFinalPath;
    public Transform[] greenFinalPath;
    public Transform[] purpleFinalPath;


    public float pawnSpreadRadius = 8f;
    public TurnState turnState = TurnState.DiceReady;
    public static LudoGameManager Instance;
    public LudoPawn selectedPawn;
    public LudoDice dice;

    public int currentDiceValue;
    [Header("UI")]
    public GameObject rankingPanel;       
    public TMP_Text rankingText; 

    [Header("Players")]
    public int playerCount = 4;
    public int currentPlayer = 0;
    public bool extraTurn = false;
    public bool waitingForPawn = false;
    public bool waitingForMove = false;
    private static readonly int[] safeCells = { 0, 13, 27, 41 };
    [Header("Ranking")]
    public List<PawnColor> finishedPlayersOrder = new List<PawnColor>();
    public bool gameEnded = false;


    



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
        //playerCount = PlayerPrefs.GetInt("PlayerCount", 4);
        playerCount = GameManager.Instance.PlayerCount;
        Debug.Log($"Ludo Player Count: {playerCount}");
        SetupPlayers();

        boardCells = new Transform[55]; 
        for (int i = 0; i < 55; i++) 
        {
            boardCells[i] = GameObject.Find($"Cell_{i:00}").transform;
        }
        Debug.Log($"Total Pawns: {allPawns.Count}");
        Debug.Log($"Current Turn: {GetCurrentPlayerColor()}");

        foreach (LudoPawn pawn in allPawns)
        {
            switch (pawn.pawnColor)
            {
                case PawnColor.Blue:
                    pawn.SetFinalPath(blueFinalPath);
                    break;

                case PawnColor.Orange:
                    pawn.SetFinalPath(orangeFinalPath);
                    break;

                case PawnColor.Green:
                    pawn.SetFinalPath(greenFinalPath);
                    break;

                case PawnColor.Purple:
                    pawn.SetFinalPath(purpleFinalPath);
                    break;
            }
        }
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
        do
        {
            currentPlayer++;
            if (currentPlayer >= playerCount)
                currentPlayer = 0;
        }
        while (finishedPlayersOrder.Contains(GetCurrentPlayerColor()));

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
            if (pawn.pawnColor == color && !pawn.isInBase)
            {
                if (CanPawnMove(pawn, currentDiceValue))
                    return true;
            }
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
            turnState = TurnState.Moving;
            StartCoroutine(EnterPawnAndFinish(pawn, start));
            return;
        }

        // حرکت مهره‌ای که روی صفحه‌ست
        if (!pawn.isInBase && currentDiceValue > 0)
        {
            if (!CanPawnMove(pawn, currentDiceValue))
            {
                Debug.Log("این مهره با این عدد تاس نمیتونه حرکت کنه (Final پر شده یا عدد زیاده)");

                if (AudioManagerMe.Instance != null)
                {
                    AudioManagerMe.Instance.PlaySound(
                        AudioManagerMe.Instance.error
                    );
                }

                return;
            }
            int move = currentDiceValue;
            bool wasSix = (move == 6);
            currentDiceValue = 0;
            turnState = TurnState.Moving;
            StartCoroutine(MoveAndFinish(pawn, move, wasSix));
        }
    }

    private IEnumerator MoveAndFinish(LudoPawn pawn, int steps, bool wasSix)
    {
        int oldCell = pawn.currentCell;

        yield return StartCoroutine(
            pawn.MoveSteps(steps, boardCells)
        );

        CheckCapture(pawn);
        CheckPlayerFinished(pawn.pawnColor);

        // فقط وقتی مهره هنوز در MainPath است
        if (pawn.finalPathIndex < 0)
        {
            ArrangePawnsAt(oldCell);
            ArrangePawnsAt(pawn.currentCell);
        }

        turnState = TurnState.DiceReady;

        if (gameEnded)
        {
            yield break;
        }

        if (wasSix)
        {
            dice.EnableRoll();
        }
        else
        {
            NextTurn();
            dice.EnableRoll();
        }
    }

    private IEnumerator EnterPawnAndFinish(LudoPawn pawn, int startCell)
    {
        if (AudioManagerMe.Instance != null)
        {
            AudioManagerMe.Instance.PlaySound(
                AudioManagerMe.Instance.pawnEnter
            );
        }
        yield return StartCoroutine(pawn.EnterBoardAnimated(startCell, boardCells[startCell]));

        CheckCapture(pawn);          // <-- اضافه شد
        ArrangePawnsAt(startCell);
        currentDiceValue = 0;
        turnState = TurnState.DiceReady;
        dice.EnableRoll();
    }
    public void ArrangePawnsAt(int cellIndex)
    {
        if (cellIndex < 0 || cellIndex >= boardCells.Length) return;

        List<LudoPawn> pawnsHere = allPawns.FindAll(
       p => !p.isInBase && !p.hasFinished && p.finalPathIndex < 0 && p.currentCell == cellIndex);

        pawnsHere.Sort((a, b) =>
        {
            int c = a.pawnColor.CompareTo(b.pawnColor);
            return c != 0 ? c : a.pawnIndex.CompareTo(b.pawnIndex);
        });

        if (pawnsHere.Count == 0) return;

        RectTransform cellRt = boardCells[cellIndex].GetComponent<RectTransform>();
        Vector2 basePos = cellRt.anchoredPosition;

        if (pawnsHere.Count == 1)
        {
            pawnsHere[0].GetComponent<RectTransform>().anchoredPosition = basePos;
            return;
        }

        int count = pawnsHere.Count;
        for (int i = 0; i < count; i++)
        {
            float angle = (2 * Mathf.PI / count) * i - Mathf.PI / 2f;
            Vector2 offset = new Vector2(
                Mathf.Cos(angle) * pawnSpreadRadius,
                Mathf.Sin(angle) * pawnSpreadRadius
            );
            pawnsHere[i].GetComponent<RectTransform>().anchoredPosition = basePos + offset;
        }
    }
    public void CheckCapture(LudoPawn movedPawn)
    {
        if (movedPawn.finalPathIndex >= 0)
        {
            return;
        }

        if (IsSafeCell(movedPawn.currentCell))
        {
            Debug.Log("خونه امنه، هیچ Capture‌ای انجام نمیشه");
            return;
        }

        List<LudoPawn> pawnsHere = allPawns.FindAll(
            p => p != movedPawn &&
                 !p.isInBase &&
                 !p.hasFinished &&
                 p.finalPathIndex < 0 &&
                 p.currentCell == movedPawn.currentCell &&
                 p.pawnColor != movedPawn.pawnColor
        );

        foreach (var enemyPawn in pawnsHere)
        {
            enemyPawn.SendHome();

            if (AudioManagerMe.Instance != null)
            {
                AudioManagerMe.Instance.PlaySound(
                    AudioManagerMe.Instance.capture
                );
            }

            Debug.Log($"{movedPawn.pawnColor} captured {enemyPawn.pawnColor}!");
        }
    }

    public bool IsSafeCell(int cellIndex)
    {
        foreach (int c in safeCells)
            if (c == cellIndex) return true;
        return false;
    }
    public bool IsFinalCellOccupied(PawnColor color, int finalIndex, LudoPawn excludePawn)
    {
        foreach (var p in allPawns)
        {
            if (p == excludePawn) continue;
            if (p.pawnColor == color && p.finalPathIndex == finalIndex)
                return true;
        }
        return false;
    }

    public bool CanPawnMove(LudoPawn pawn, int steps)
    {
        // اول اورشوت را چک کن
        if (!pawn.CanMoveWithDice(steps))
            return false;

        // اگر مقصد داخل Final نیست، حرکت مجاز است
        int targetIndex = pawn.GetTargetFinalIndex(steps);

        if (targetIndex < 0)
            return true;

        // اگر مقصد داخل Final است، نباید اشغال باشد
        if (targetIndex < pawn.finalPath.Length)
        {
            if (IsFinalCellOccupied(
                pawn.pawnColor,
                targetIndex,
                pawn))
            {
                Debug.Log(
                    $"{pawn.pawnColor} Final_{targetIndex} اشغال است."
                );

                return false;
            }
        }

        return true;
    }
    private void CheckPlayerFinished(PawnColor color)
    {
        // اگه قبلاً تو لیست رتبه‌بندی بود، دیگه لازم نیست چک کنیم
        if (finishedPlayersOrder.Contains(color))
            return;

        int finishedCount = allPawns.FindAll(
            p => p.pawnColor == color && p.hasFinished).Count;

        if (finishedCount == 4)
        {
            finishedPlayersOrder.Add(color);
            Debug.Log($"{color} تمام مهره‌هاشو برد! رتبه: {finishedPlayersOrder.Count}");

            if (AudioManagerMe.Instance != null)
            {
                AudioManagerMe.Instance.PlaySound(
                    AudioManagerMe.Instance.win
                );
            }

            CheckGameEnd();
        }
    }

    private void CheckGameEnd()
    {
        if (gameEnded) return;

        if (finishedPlayersOrder.Count >= playerCount - 1)
        {
            gameEnded = true;
            Debug.Log("بازی تموم شد! پنل رتبه‌بندی باز میشه.");
            ShowRankingPanel();
        }
    }

    private void ShowRankingPanel()
    {
        if (AudioManagerMe.Instance != null)
        {
            AudioManagerMe.Instance.PlaySound(
                AudioManagerMe.Instance.rankingOpen
            );
        }
        if (rankingPanel != null)
            rankingPanel.SetActive(true);

        if (rankingText == null)
            return;

        string result = " رتبه‌بندی نهایی\n\n";

        // بازیکن‌هایی که بازی را تمام کرده‌اند
        for (int i = 0; i < finishedPlayersOrder.Count; i++)
        {
            result += $"{i + 1}. {GetPlayerName(finishedPlayersOrder[i])}\n";
        }

        // بازیکن باقی‌مانده
        for (int i = 0; i < playerCount; i++)
        {
            PawnColor color = IndexToColor(i);

            if (!finishedPlayersOrder.Contains(color))
            {
                result += $"{finishedPlayersOrder.Count + 1}. {GetPlayerName(color)}\n";
            }
        }

        rankingText.text = result;

        Debug.Log(result);
    }
    private string GetPlayerName(PawnColor color)
    {
        switch (color)
        {
            case PawnColor.Blue:
                return "ابی";

            case PawnColor.Orange:
                return "نارنجی";

            case PawnColor.Green:
                return "سبز";

            case PawnColor.Purple:
                return "بنفش";

            default:
                return color.ToString();
        }
    }

    private PawnColor IndexToColor(int index)
    {
        switch (index)
        {
            case 0: return PawnColor.Blue;
            case 1: return PawnColor.Orange;
            case 2: return PawnColor.Green;
            case 3: return PawnColor.Purple;
            default: return PawnColor.Blue;
        }
    }
    private void SetupPlayers()
    {
        foreach (LudoPawn pawn in allPawns)
        {
            bool shouldBeActive = true;

            switch (playerCount)
            {
                case 2:
                    shouldBeActive =
                        pawn.pawnColor == PawnColor.Blue ||
                        pawn.pawnColor == PawnColor.Orange;
                    break;

                case 3:
                    shouldBeActive =
                        pawn.pawnColor == PawnColor.Blue ||
                        pawn.pawnColor == PawnColor.Orange ||
                        pawn.pawnColor == PawnColor.Green;
                    break;

                case 4:
                    shouldBeActive = true;
                    break;
            }

            pawn.gameObject.SetActive(shouldBeActive);
        }
    }
    public void ResetLudoGameData()
    {
        currentPlayer = 0;
        currentDiceValue = 0;

        selectedPawn = null;

        extraTurn = false;
        waitingForPawn = false;
        waitingForMove = false;

        turnState = TurnState.DiceReady;

        finishedPlayersOrder.Clear();

        gameEnded = false;

        Debug.Log("Ludo game data reset.");
    }

}