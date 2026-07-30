using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BoardConnection
{
    public int startCell;
    public int endCell;
}

public class SnakeAndLadderManager : MonoBehaviour
{
    public static SnakeAndLadderManager Instance;

    public List<BoardConnection> ladders = new List<BoardConnection>();
    public List<BoardConnection> snakes = new List<BoardConnection>();

    private void Awake()
    {
        Instance = this;
    }

    public bool TryGetDestination(int currentCell, out int destination)
    {
        foreach (var ladder in ladders)
        {
            if (ladder.startCell == currentCell)
            {
                destination = ladder.endCell;
                return true;
            }
        }

        foreach (var snake in snakes)
        {
            if (snake.startCell == currentCell)
            {
                destination = snake.endCell;
                return true;
            }
        }

        destination = currentCell;
        return false;
    }
}
