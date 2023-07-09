using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonaLisa : MonoBehaviour, Interactable
{
    public List<EnemyPatrolling2> newGuards = new List<EnemyPatrolling2>();

    private void Start()
    {
        foreach (EnemyPatrolling2 guard in newGuards)
        {
            guard.gameObject.SetActive(false);
        }
    }
    public void Interact()
    {
        Debug.Log("OOOH THAT'S MY TREASURE");
        gameObject.SetActive(false);
        GameManager.Instance.SetGamePhase(GameManager.GamePhase.Escape);
        foreach (EnemyPatrolling2 guard in newGuards)
        {
            guard.gameObject.SetActive(true);
        }
    }

}
