using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //public void Resume() {

    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;

    //    pauseMenuUI.SetActive(false);
    //    Time.timeScale = 1f;
    //    GameIsPaused = false;
    //}

    public void Start() {
        Debug.Log("GAME OVER SCREEN REACHED");
    }

    public void RestartLevel() {
        Debug.Log("RESTARTING SCENE");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
