using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
        }
    }

    public void Mainmenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Levels()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {

        Application.Quit();
    }
}
