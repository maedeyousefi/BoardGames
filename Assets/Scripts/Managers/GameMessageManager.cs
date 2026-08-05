using TMPro;
using UnityEngine;

public class GameMessageManager : MonoBehaviour
{
    public static GameMessageManager Instance;

    public TMP_Text messageText;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowMessage(string message)
    {
        messageText.text = message;
    }
    public string GetPlayerName(int player)
    {
        switch (player)
        {
            case 1:
                return " blue";
            case 2:
                return "green";
            case 3:
                return "purple";
            case 4:
                return "orange";
            default:
                return "بازیکن";
        }
    }
}