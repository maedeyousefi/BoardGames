using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum PawnColor { Blue, Orange, Green, Purple }
public class LudoPawn : MonoBehaviour, IPointerClickHandler
{
    [Header("Final Path")]
    public Transform[] finalPath;
    public int finalPathIndex = -1;
    [Header("Pawn Info")]
    public PawnColor pawnColor;
    public int pawnIndex;
    [Header("State")]
    public bool isInHome = true;
    public bool hasFinished = false;
    [Header("Path")]
    public int pathIndex = -1;


    public bool isInBase = true;
    public int currentCell = -1;
    public bool isSelected = false;
    public Button rollButton;
    public Transform homeSlot; // موقعیت اصلی این مهره تو هوم، از Inspector وصلش کن


    public IEnumerator EnterBoardAnimated(int startCell, Transform targetCell)
    {
        isInBase = false;
        isInHome = false;
        currentCell = startCell;
        pathIndex = startCell;

        RectTransform rt = GetComponent<RectTransform>();
        Transform boardParent = targetCell.parent;

        // موقعیت فعلی رو نسبت به پرنت جدید (برد) محاسبه کن قبل از عوض کردن parent
        Vector2 worldStart = rt.position;
        transform.SetParent(boardParent, true); // true یعنی موقعیت جهانی حفظ بشه
        transform.SetAsLastSibling();

        RectTransform targetRt = targetCell.GetComponent<RectTransform>();
        Vector2 start = rt.anchoredPosition;
        Vector2 end = targetRt.anchoredPosition;

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / stepDuration;
            float clampedT = Mathf.Clamp01(t);

            Vector2 pos = Vector2.Lerp(start, end, clampedT);
            float jump = jumpHeight * 4f * clampedT * (1f - clampedT);
            pos.y += jump;

            rt.anchoredPosition = pos;
            yield return null;
        }

        rt.anchoredPosition = end;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (hasFinished)
        {
            Debug.Log($"{pawnColor} Pawn {pawnIndex} already finished.");
            return;
        }

        LudoGameManager.Instance.SelectPawn(this);
    }

    private bool CanMoveToFinal(int steps)
    {
        int entryCell = GetFinalEntryCell();

        // اگر هنوز به ورودی Final نرسیده، مشکلی نیست
        if (currentCell != entryCell)
            return true;

        // تعداد خانه‌های باقی‌مانده Final
        int remainingFinalSteps = finalPath.Length - 1 - finalPathIndex;

        return steps <= remainingFinalSteps;
    }
    public float jumpHeight = 20f;
    public float stepDuration = 0.25f;

    public IEnumerator MoveSteps(int steps, Transform[] cells)
    {
        RectTransform rt = GetComponent<RectTransform>();

        // اگر مهره روی ورودی Final است، بررسی کن عدد تاس کافی باشد
        if (finalPathIndex == -1 && currentCell == GetFinalEntryCell())
        {
            int remainingSteps = finalPath.Length;

            if (steps > remainingSteps)
            {
                Debug.Log(
                    $"{pawnColor} - عدد تاس کافی نیست! " +
                    $"تاس: {steps} | خانه‌های باقی‌مانده Final: {remainingSteps}"
                );

                yield break;
            }
        }
        for (int i = 0; i < steps; i++)
        {
            Transform targetCell;

            // ==========================================
            // 1️⃣ اگر مهره از قبل داخل Final Path است
            // ==========================================
            if (finalPathIndex >= 0)
            {
                finalPathIndex++;

                // اگر از آخر Final رد شد
                if (finalPathIndex >= finalPath.Length)
                {
                    finalPathIndex = finalPath.Length - 1;
                    hasFinished = true;

                    Debug.Log($"{pawnColor} pawn finished!");
                    yield break;
                }

                targetCell = finalPath[finalPathIndex];
            }
            else
            {
                // ==========================================
                // 2️⃣ اگر مهره هنوز در Main Path است
                // ==========================================

                // اگر مهره همین الان روی خانه ورود Final است،
                // حرکت بعدی باید مستقیماً وارد Final شود.
                if (currentCell == GetFinalEntryCell())
                {
                    if (finalPath == null || finalPath.Length == 0)
                    {
                        Debug.LogError(
                            $"{pawnColor} Pawn {pawnIndex}: Final Path وصل نشده!"
                        );

                        yield break;
                    }

                    finalPathIndex = 0;
                    targetCell = finalPath[finalPathIndex];

                    transform.SetParent(targetCell.parent, true);
                    transform.SetAsLastSibling();
                }
                else
                {
                    // حرکت عادی در Main Path
                    int nextCell = currentCell + 1;

                    if (nextCell >= cells.Length)
                        nextCell = 0;

                    currentCell = nextCell;

                    targetCell = cells[currentCell];

                    // اگر الان به خانه ورود Final رسیدیم،
                    // حرکت بعدی وارد Final خواهد شد.
                    // پس فعلاً همین خانه را نمایش می‌دهیم.
                }
            }

            // ==========================================
            // 3️⃣ حرکت نرم + پرش مهره
            // ==========================================

            RectTransform target =
                targetCell.GetComponent<RectTransform>();

            Vector3 start = rt.position;
            Vector3 end = target.position;

            float t = 0f;

            while (t < 1f)
            {
                t += Time.deltaTime / stepDuration;

                float clampedT = Mathf.Clamp01(t);

                Vector3 pos =
                    Vector3.Lerp(start, end, clampedT);

                float jump =
                    jumpHeight *
                    4f *
                    clampedT *
                    (1f - clampedT);

                pos.y += jump;

                rt.position = pos;

                yield return null;
            }

            rt.position = end;
        }
    }
      
    private int GetFinalEntryCell()
    {
        switch (pawnColor)
        {
            case PawnColor.Blue:
                return 40;

            case PawnColor.Orange:
                return 26;

            case PawnColor.Green:
                return 12;

            case PawnColor.Purple:
                return 54;
        }

        return -1;
    }

    public void SendHome()
    {
        isInBase = true;
        isInHome = true;
        currentCell = -1;
        pathIndex = -1;

        RectTransform rt = GetComponent<RectTransform>();
        transform.SetParent(homeSlot.parent, true);
        rt.anchoredPosition = homeSlot.GetComponent<RectTransform>().anchoredPosition;
    }
    public void SetFinalPath(Transform[] path)
    {
        finalPath = path;
        finalPathIndex = -1;
    }
}