using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private void Start()
    {
        Time.timeScale = 1f;
    }
    public void PlayGame1()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void PlayGame2()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void PlayGame3()
    {
        SceneManager.LoadSceneAsync(3);
    }

    public void Playgame4()
    {
        SceneManager.LoadSceneAsync(4);
    }

    public void back()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
        
    }
}
