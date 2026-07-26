using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BoardGenerator : MonoBehaviour
{
    public GameObject cellPrefab;
    public Transform boardContainer;

    public int columns = 10;
    public int rows = 10;
    public float cellSize = 100f;
    public static Dictionary<int, Vector2> cellPositions = new Dictionary<int, Vector2>();

    void Awake()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
        int cellNumber = 1;
        cellPositions.Clear();

        for (int row = 0; row < rows; row++)
        {
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

                // ذخیره موقعیت این خونه
                cellPositions[cellNumber] = new Vector2(xPos, yPos);

                cellNumber++;
            }
        }
    }
}