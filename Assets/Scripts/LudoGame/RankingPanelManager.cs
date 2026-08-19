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
        ResetGameData();

        SceneManager.LoadScene(gameSceneName);
    }

    // Main Menu
    public void MainMenu()
    {
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