using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }

        

        void Pause()
        {
            pauseMenuUI.SetActive(true);    //abilita la visualizzazione del panel della pausa
            Time.timeScale = 0f;    //per bloccare il gioco
            GameIsPaused = true;
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void restartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quitLevel() {
        SceneManager.LoadScene("SelectLevelScene");
    }
}
