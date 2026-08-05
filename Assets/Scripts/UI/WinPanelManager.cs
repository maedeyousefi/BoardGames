using TMPro;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanelManager : MonoBehaviour
{
    public static WinPanelManager Instance;

    [Header("UI")]
    public GameObject winPanel;
    public TMP_Text rankingText;

    [Header("Animation")]
    public RectTransform panelTransform;

    public float animationTime = 0.25f;

    private void Awake()
    {
        Instance = this;
        winPanel.SetActive(false);
    }

    private string GetPlayerName(int player) 
    { 
        switch (player) 
        { 
            case 1: return " Blue \n";
            case 2: return " Green \n";
            case 3: return " Purple \n";
            case 4: return " Orange \n";
            default: return "Player " + player; } }
    public void ShowWinPanel()
    {
        winPanel.SetActive(true);

        StartCoroutine(AnimatePanel());

        AudioManager.Instance.PlaySound(AudioManager.Instance.win);

        Time.timeScale = 0f;

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

        winPanel.SetActive(false);
        GameManager.Instance.ResetGameData();
        PawnMover.cellOccupants.Clear();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
    }
    public void MainMenu() 
    { 
        Time.timeScale = 1f;
        winPanel.SetActive(false);
        GameManager.Instance.ResetGameData();
        PawnMover.cellOccupants.Clear();
        SceneManager.LoadScene("MainMenu"); 
    }
    private IEnumerator AnimatePanel()
    {
        panelTransform.localScale = Vector3.zero;

        float time = 0f;

        while (time < animationTime)
        {
            float t = time / animationTime;

            panelTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * 1.1f, t);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        panelTransform.localScale = Vector3.one * 1.1f;

        time = 0f;

        while (time < 0.08f)
        {
            float t = time / 0.08f;

            panelTransform.localScale = Vector3.Lerp(Vector3.one * 1.1f, Vector3.one, t);

            time += Time.unscaledDeltaTime;
            yield return null;
        }

        panelTransform.localScale = Vector3.one;
    }
}