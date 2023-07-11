using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EnemyPatrolling2.OnGameOver += EnemyPatrolling2_OnGameOver;
        GameManager.Instance.OnGamePhaseChanged += Instance_OnGamePhaseChanged;
    }

    private void Instance_OnGamePhaseChanged(GameManager.GamePhase obj)
    {
        if(obj == GameManager.GamePhase.Escape)
        {
            transform.Find("Escape - Canvas").gameObject.SetActive(true);
        }
        
    }

    private void EnemyPatrolling2_OnGameOver(object sender, System.EventArgs e) {
        EnemyPatrolling2.OnGameOver -= EnemyPatrolling2_OnGameOver;
        GameManager.Instance.isGameOver = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;    //per bloccare il gioco
        transform.Find("Game Over - Canvas").gameObject.SetActive(true);
    }

    private void OnDestroy() {
        EnemyPatrolling2.OnGameOver -= EnemyPatrolling2_OnGameOver;
        GameManager.Instance.OnGamePhaseChanged -= Instance_OnGamePhaseChanged;
    }

}
