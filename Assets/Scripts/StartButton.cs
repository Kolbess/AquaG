using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void ShowCredentials()
    {
        SceneManager.LoadScene("Credentials");
    }

    public void HideCredentials()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
