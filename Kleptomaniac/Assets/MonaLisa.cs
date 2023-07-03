using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonaLisa : MonoBehaviour, Interactable
{
    public void Interact()
    {
        Debug.Log("OOOH THAT'S MY TREASURE");
        gameObject.SetActive(false);
        GameManager.Instance.SetPhaseEscape();
    }

}
