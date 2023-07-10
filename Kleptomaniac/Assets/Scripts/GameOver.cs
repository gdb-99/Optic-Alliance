using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    
    public void Start() {
        Debug.Log("GAME OVER SCREEN REACHED");
    }

    public void RestartLevel() {
        Debug.Log("RESTARTING SCENE");

        //Fai ripartire il gioco:
        Time.timeScale = 1f;

        //Carica la nuova scena:
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        //Togli il cursore:
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame(){

        Debug.Log("QUIT SCENE");

        //Fai ripartire il gioco:
        Time.timeScale = 1f;

        //Carica la nuova scena:
        SceneManager.LoadScene("SelectLevelScene");

        //Mostra il cursore:
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
}
