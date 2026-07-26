using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PawnMover : MonoBehaviour, IPointerClickHandler
{
    public DiceManager diceManager;
    public int currentCell = 1;
    public int playerNumber = 1;

    public float moveSpeed = 0.2f;
    public float hopHeight = 40f;

    private bool isMoving = false;
    private Vector2 currentOffset = Vector2.zero;

    // دفترچه: هر خونه -> لیست مهره‌های داخلش
    public static Dictionary<int, List<PawnMover>> cellOccupants = new Dictionary<int, List<PawnMover>>();

    // موقعیت‌های ممکن وقتی چند مهره توی یه خونه‌ان
    private static readonly Vector2[] multiOffsets = new Vector2[]
    {
        new Vector2(-20, 20),
        new Vector2(20, 20),
        new Vector2(-20, -20),
        new Vector2(20, -20)
    };

    void Start()
    {
        transform.SetAsLastSibling();
        RegisterOnCell(currentCell);
        ArrangeCell(currentCell);

        RectTransform rt = GetComponent<RectTransform>();
        rt.anchoredPosition = BoardGenerator.cellPositions[currentCell] + currentOffset;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isMoving) return;

        if (GameManager.Instance.CurrentPlayerTurn != playerNumber)
        {
            Debug.Log($"نوبت تو نیست! نوبت فعلی: {GameManager.Instance.CurrentPlayerTurn}");
            return;
        }

        if (!diceManager.IsRollAvailable())
        {
            Debug.Log("اول باید تاس بزنی!");
            return;
        }

        int steps = diceManager.ConsumeRoll();
        StartCoroutine(MoveSteps(steps));
    }

    private IEnumerator MoveSteps(int steps)
    {
        isMoving = true;

        // از خونه‌ی فعلی خارج شو
        UnregisterFromCell(currentCell);
        ArrangeCell(currentCell); // بقیه‌ای که موندن رو دوباره بچین

        for (int i = 0; i < steps; i++)
        {
            currentCell++;
            bool isLastStep = (i == steps - 1);

            if (!BoardGenerator.cellPositions.ContainsKey(currentCell))
                continue;

            Vector2 targetPos;

            if (isLastStep)
            {
                // وارد خونه‌ی نهایی شو و موقعیت رو بر اساس تعداد مهره‌ها تنظیم کن
                RegisterOnCell(currentCell);
                ArrangeCell(currentCell);
                targetPos = BoardGenerator.cellPositions[currentCell] + currentOffset;
            }
            else
            {
                // مسیر بین راه، وسط خونه حرکت کن
                targetPos = BoardGenerator.cellPositions[currentCell];
            }

            yield return StartCoroutine(MoveToPosition(targetPos));
        }

        isMoving = false;

        diceManager.EnableRoll();
        GameManager.Instance.NextTurn();
    }

    private IEnumerator MoveToPosition(Vector2 targetPos)
    {
        RectTransform rt = GetComponent<RectTransform>();
        Vector2 startPos = rt.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < moveSpeed)
        {
            float progress = elapsed / moveSpeed;
            Vector2 linearPos = Vector2.Lerp(startPos, targetPos, progress);
            float hop = Mathf.Sin(progress * Mathf.PI) * hopHeight;
            rt.anchoredPosition = linearPos + new Vector2(0f, hop);
            elapsed += Time.deltaTime;
            yield return null;
        }

        rt.anchoredPosition = targetPos;
    }

    private void RegisterOnCell(int cell)
    {
        if (!cellOccupants.ContainsKey(cell))
            cellOccupants[cell] = new List<PawnMover>();

        if (!cellOccupants[cell].Contains(this))
            cellOccupants[cell].Add(this);
    }

    private void UnregisterFromCell(int cell)
    {
        if (cellOccupants.ContainsKey(cell))
        {
            cellOccupants[cell].Remove(this);
            if (cellOccupants[cell].Count == 0)
                cellOccupants.Remove(cell);
        }
    }

    // موقعیت همه‌ی مهره‌های یه خونه رو دوباره حساب می‌کنه
    private void ArrangeCell(int cellNumber)
    {
        if (!cellOccupants.ContainsKey(cellNumber)) return;
        if (!BoardGenerator.cellPositions.ContainsKey(cellNumber)) return;

        List<PawnMover> occupants = cellOccupants[cellNumber];
        Vector2 basePos = BoardGenerator.cellPositions[cellNumber];

        for (int i = 0; i < occupants.Count; i++)
        {
            Vector2 offset = occupants.Count == 1 ? Vector2.zero : multiOffsets[i % multiOffsets.Length];
            occupants[i].currentOffset = offset;

            // اگه این مهره خودمون نیستیم (یعنی مهره‌ای که از قبل اونجا نشسته)، فوراً جاش رو عوض کن
            if (occupants[i] != this)
            {
                RectTransform otherRt = occupants[i].GetComponent<RectTransform>();
                otherRt.anchoredPosition = basePos + offset;
            }
        }
    }
}