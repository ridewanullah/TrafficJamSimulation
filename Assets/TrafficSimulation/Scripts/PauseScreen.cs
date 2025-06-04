using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PausePanel;
    public void Setup()
    {
        gameObject.SetActive(true);
    }
    public void pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void Resume()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
