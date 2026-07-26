using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerSetupManager : MonoBehaviour
{
    [SerializeField] Button twoPlayersButton;
    [SerializeField] Button threePlayersButton;
    [SerializeField] Button fourPlayersButton;
    [SerializeField] GameObject warningPanel;

    private bool hasSelectedPlayerCount = false;
    private Color originalColor2;
    private Color originalColor3;
    private Color originalColor4;
    private Coroutine warningCoroutine;


    private void Start()
    {
        // رنگ اصلی هر دکمه رو همون اول ذخیره می‌کنیم
        originalColor2 = twoPlayersButton.image.color;
        originalColor3 = threePlayersButton.image.color;
        originalColor4 = fourPlayersButton.image.color;
    }

    public void Select2Players()
    {
        GameManager.Instance.PlayerCount = 2;
        hasSelectedPlayerCount = true;
        HighlightSelected(twoPlayersButton, originalColor2);
    }
    public void Select3Players()
    {
        GameManager.Instance.PlayerCount = 3;
        hasSelectedPlayerCount = true;
        HighlightSelected(threePlayersButton, originalColor3);
    }

    public void Select4Players()
    {
        GameManager.Instance.PlayerCount = 4;
        hasSelectedPlayerCount = true;
        HighlightSelected(fourPlayersButton, originalColor4);
    }

    public void StartGame()
    {
        if (!hasSelectedPlayerCount)
        {
            ShowWarning();
            return;
        }

        if (GameManager.Instance.SelectedGame == GameType.Snake)
        {
            SceneManager.LoadScene("SnakeGame");
        }
        else
        {
            SceneManager.LoadScene("LudoGame");
        }
    }

    private void ShowWarning()
    {
        warningPanel.SetActive(true);

        // اگه قبلاً یه Coroutine در حال اجرا بود، متوقفش کن
        if (warningCoroutine != null)
        {
            StopCoroutine(warningCoroutine);
        }

        warningCoroutine = StartCoroutine(HideWarningAfterDelay(2.5f));
    }

    private IEnumerator HideWarningAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        warningPanel.SetActive(false);
    }
    private void HighlightSelected(Button selectedButton, Color selectedOriginalColor)
    {
        twoPlayersButton.image.color = originalColor2;
        threePlayersButton.image.color = originalColor3;
        fourPlayersButton.image.color = originalColor4;

        selectedButton.image.color = Color.Lerp(selectedOriginalColor, Color.white, 0.4f);

        warningPanel.SetActive(false);
    }
}