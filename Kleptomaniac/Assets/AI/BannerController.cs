using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BannerController : MonoBehaviour
{
    public UnityEngine.UI.Image uiImage;
    //public Animation imageAnimation;
    // Start is called before the first frame update
    void Start()
    {
        uiImage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.L)) { 
        //  alarmLight.enabled = true;
        // }
    }

    private void OnGamePhaseChanged(GameManager.GamePhase newPhase)
    {
        // Controlla la nuova fase di gioco e abilita o disabilita gli oggetti di conseguenza
        if (newPhase == GameManager.GamePhase.Escape)
        {
            uiImage.enabled = true;
            uiImage.GetComponent<Animator>().enabled = true;
            // Abilita gli oggetti per la fase di furto
            //gameObject.SetActive(true);
        }
    }

}


