using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanelManager : MonoBehaviour
{
    public static WinPanelManager Instance;

    [Header("UI")]
    public GameObject winPanel;
    public TMP_Text rankingText;

    private void Awake()
    {
        Instance = this;
        winPanel.SetActive(false);
    }

    private string GetPlayerName(int player) 
    { 
        switch (player) 
        { 
            case 1: return " Blue";
            case 2: return " Green";
            case 3: return " Purple";
            case 4: return " Orange";
            default: return "Player " + player; } }
    public void ShowWinPanel()
    {
        winPanel.SetActive(true);

        string result = "";

        for (int i = 0; i < GameManager.Instance.finishedPlayers.Count; i++)
        {
            int player = GameManager.Instance.finishedPlayers[i];

            string medal = "";

            switch (i)
            {
                case 0:
                    medal = "1.";
                    break;
                case 1:
                    medal = "2.";
                    break;
                case 2:
                    medal = "3.";
                    break;
                default:
                    medal = "4.";
                    break;
            }

            result += $"{medal} {GetPlayerName(player)}";
        }

        rankingText.text = result;
    }
    public void Replay() 
    { 
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
    public void MainMenu() 
    { 
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
    }
}