using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
public enum PawnColor { Blue, Orange, Green, Purple }
public class LudoPawn : MonoBehaviour , IPointerClickHandler
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

    public void EnterBoard(int startCell, Transform targetCell)
    {
        isInBase = false;
        isInHome = false;

        currentCell = startCell;
        pathIndex = startCell;

        RectTransform rt = GetComponent<RectTransform>();
        RectTransform targetRt = targetCell.GetComponent<RectTransform>();

        transform.SetParent(targetCell.parent, false);

        rt.anchoredPosition = targetRt.anchoredPosition;

        transform.SetAsLastSibling();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isInBase)
            return;

        LudoGameManager.Instance.SelectPawn(this);
    }
    public IEnumerator MoveSteps(int steps, Transform[] cells)
    {
        for (int i = 0; i < steps; i++)
        {
            currentCell++;

            if (currentCell >= cells.Length)
                currentCell = 0;

            RectTransform rt = GetComponent<RectTransform>();
            RectTransform target = cells[currentCell].GetComponent<RectTransform>();

            Vector2 start = rt.anchoredPosition;
            Vector2 end = target.anchoredPosition;

            float t = 0;

            while (t < 1)
            {
                t += Time.deltaTime * 5f;
                rt.anchoredPosition = Vector2.Lerp(start, end, t);

                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}