using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadPlayerSetup()
    {
        SceneManager.LoadScene("PlayerSetup");
    }

    public void SelectSnakeGame()
    {
        GameManager.Instance.SelectedGame = GameType.Snake;
        LoadPlayerSetup();
    }

    public void SelectLudoGame()
    {
        GameManager.Instance.SelectedGame = GameType.Ludo;
        LoadPlayerSetup();
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }
}