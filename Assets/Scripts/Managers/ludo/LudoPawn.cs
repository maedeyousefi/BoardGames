using UnityEngine;
public enum PawnColor { Blue, Orange, Green, Purple }
public class LudoPawn : MonoBehaviour 
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

    public void EnterBoard(int startCell, Transform targetCell)
    {
        isInBase = false;
        currentCell = startCell;

        RectTransform rt = GetComponent<RectTransform>();
        RectTransform targetRt = targetCell.GetComponent<RectTransform>();

        // مهره را زیر MainPath قرار بده
        transform.SetParent(targetCell.parent, false);

        // دقیقاً روی خانه قرار بگیرد
        rt.anchoredPosition = targetRt.anchoredPosition;

        // روی خانه‌ها نمایش داده شود
        transform.SetAsLastSibling();
    }
}