using UnityEngine;

public class SnakeGameSetup : MonoBehaviour
{
    public GameObject pawnPlayer1;
    public GameObject pawnPlayer2;
    public GameObject pawnPlayer3;
    public GameObject pawnPlayer4;

    void Awake()
    {
        int playerCount = GameManager.Instance.PlayerCount;

        pawnPlayer1.SetActive(playerCount >= 1);
        pawnPlayer2.SetActive(playerCount >= 2);
        pawnPlayer3.SetActive(playerCount >= 3);
        pawnPlayer4.SetActive(playerCount >= 4);
    }
}