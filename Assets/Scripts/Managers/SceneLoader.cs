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
        Debug.Log("Snake Button Clicked");

        GameManager.Instance.SelectedGame = GameType.Snake;
        LoadPlayerSetup();
    }

    public void SelectLudoGame()
    {
        Debug.Log("Ludo Button Clicked");

        GameManager.Instance.SelectedGame = GameType.Ludo;
        LoadPlayerSetup();
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game Closed");
    }
}