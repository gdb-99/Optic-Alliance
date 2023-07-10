using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    [SerializeField] GameObject[] clips; // drag and add audio clips in the inspector
    [SerializeField] GameObject panel;
    GameObject thisClip;
    private AudioSource audioItem;
    private bool flag;

    // Start is called before the first frame update
    void Start()
    {
        flag = false;
        audioItem = GetComponent<AudioSource>();
        clips[0].SetActive(true);
        thisClip = clips[0];
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (GameIsPaused == true)
        {
            //abbassa il volume di thisClip
        }
        else
        {
            //volume normale di thisClip
        }
        */

        if (GameManager.Instance.currentPhase == GameManager.GamePhase.Escape)
        {
            if (!flag)
            {
                flag = true;
                audioItem.Play();
            }
            clips[0].SetActive(false);  //stealth
            panel.SetActive(true);
            clips[1].SetActive(true);   //escape
        }
    }
}
