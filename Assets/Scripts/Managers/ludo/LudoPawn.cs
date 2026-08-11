using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum PawnColor { Blue, Orange, Green, Purple }
public class LudoPawn : MonoBehaviour, IPointerClickHandler
{
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
        LudoGameManager.Instance.SelectPawn(this);
    }

    public float jumpHeight = 20f;
    public float stepDuration = 0.25f;

    public IEnumerator MoveSteps(int steps, Transform[] cells)
    {
        RectTransform rt = GetComponent<RectTransform>();

        for (int i = 0; i < steps; i++)
        {
            currentCell++;
            if (currentCell >= cells.Length)
                currentCell = 0;

            RectTransform target = cells[currentCell].GetComponent<RectTransform>();
            Vector2 start = rt.anchoredPosition;
            Vector2 end = target.anchoredPosition;

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / stepDuration;
                float clampedT = Mathf.Clamp01(t);

                // مسیر افقی/عمودی خطی
                Vector2 pos = Vector2.Lerp(start, end, clampedT);

                // پرش با فرمول سهمی: بیشترین ارتفاع وسط مسیره
                float jump = jumpHeight * 4f * clampedT * (1f - clampedT);
                pos.y += jump;

                rt.anchoredPosition = pos;
                yield return null;
            }

            rt.anchoredPosition = end; // برای اطمینان دقیقاً روی خونه بشینه
            yield return new WaitForSeconds(0.05f);
        }
    }
}