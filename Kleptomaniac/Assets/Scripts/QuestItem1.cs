using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem1 : MonoBehaviour, Interactable
{
    public void Interact()
    {
        GameManager.Instance.quest1 = true;
        transform.Find("Fish00_low__mLight").gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
