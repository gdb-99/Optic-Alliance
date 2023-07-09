using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnemyPatrolling2.OnGameOver += EnemyPatrolling2_OnGameOver;
    }

    private void EnemyPatrolling2_OnGameOver(object sender, System.EventArgs e) {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;    //per bloccare il gioco
        transform.Find("Game Over - Canvas").gameObject.SetActive(true);
    }

}
