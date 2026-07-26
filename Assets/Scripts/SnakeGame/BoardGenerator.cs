using UnityEngine;
using TMPro;

public class BoardGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform boardContainer;

    public int columns = 10;
    public int rows = 10;
    public float cellSize = 150f;

    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
        int cellNumber = 1;

        for (int row = 0; row < rows; row++)
        {
            // اگه ردیف زوج بود، از چپ به راست بشمار
            // اگه فرد بود، از راست به چپ بشمار (مارپیچی)
            bool isReversed = row % 2 == 1;

            for (int col = 0; col < columns; col++)
            {
                int actualCol = isReversed ? (columns - 1 - col) : col;

                float xPos = actualCol * cellSize;
                float yPos = row * cellSize;

                GameObject newCell = Instantiate(cellPrefab, boardContainer);
                newCell.name = "Cell_" + cellNumber;

                RectTransform rt = newCell.GetComponent<RectTransform>();
                rt.anchoredPosition = new Vector2(xPos, yPos);

                TextMeshProUGUI numberText = newCell.GetComponentInChildren<TextMeshProUGUI>();
                numberText.text = cellNumber.ToString();

                cellNumber++;
            }
        }
    }
}
