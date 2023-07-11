using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PremiseController : MonoBehaviour
{

    private static bool firstTime = true;
    [SerializeField] TutorialStepSO tutorialData;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] TextMeshProUGUI tutorialInfoText;
    [SerializeField] TextMeshProUGUI tutorialTitleText;

    // Start is called before the first frame update
    void Start()
    {
        if(tutorialData.premiseScene && PremiseController.firstTime){
        tutorialInfoText.text = tutorialData.info;
        tutorialTitleText.text = tutorialData.title;
        tutorialPanel.SetActive(true);

        Time.timeScale = 0.0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        }
    }
    
    public void ActivatePanel(){
        tutorialInfoText.text = tutorialData.info;
        tutorialTitleText.text = tutorialData.title;
        tutorialPanel.SetActive(true);

        Time.timeScale = 0.0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseTutorialPanel() { 
        tutorialPanel.SetActive(false); 
        gameObject.SetActive(false);

        Time.timeScale = 1.0f;

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    public void SetFirstTime(){
        PremiseController.firstTime = false;
    }

}
