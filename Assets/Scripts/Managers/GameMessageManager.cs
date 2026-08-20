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
                return " ابی";
            case 2:
                return "سبز";
            case 3:
                return "بنفش";
            case 4:
                return "نارنجی";
            default:
                return "بازیکن";
        }
    }
}