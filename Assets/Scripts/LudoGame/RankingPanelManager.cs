using UnityEngine;
using UnityEngine.SceneManagement;

public class RankingPanelManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string gameSceneName = "LudoGame";
    public string mainMenuSceneName = "MainMenu";

    // Replay
    public void Replay()
    {
        if (AudioManagerMe.Instance != null)
        {
            AudioManagerMe.Instance.PlaySound(
                AudioManagerMe.Instance.buttonClick
            );
        }
        ResetGameData();

        SceneManager.LoadScene(gameSceneName);
    }

    // Main Menu
    public void MainMenu()
    {
        if (AudioManagerMe.Instance != null)
        {
            AudioManagerMe.Instance.PlaySound(
                AudioManagerMe.Instance.buttonClick
            );
        }
        ResetGameData();

        SceneManager.LoadScene(mainMenuSceneName);
    }

    private void ResetGameData()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGameData();
        }

        if (LudoGameManager.Instance != null)
        {
            LudoGameManager.Instance.ResetLudoGameData();
        }
    }
}