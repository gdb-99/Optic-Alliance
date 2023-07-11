using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialStepController : MonoBehaviour
{

    [SerializeField] TutorialStepSO tutorialData;
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] TextMeshProUGUI tutorialInfoText;
    [SerializeField] TextMeshProUGUI tutorialTitleText;

    int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        tutorialInfoText.text = tutorialData.info;
        tutorialTitleText.text = tutorialData.title;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(tutorialData.title != "Look Out!" || counter == 1){

                tutorialPanel.SetActive(true);

                Time.timeScale = 0.0f;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                counter = 0;
            }
            else if(tutorialData.title == "Look Out!"){
                counter = 1;
            }
        }
    }

    public void CloseTutorialPanel() { 
        tutorialPanel.SetActive(false); 
        gameObject.SetActive(false);

        Time.timeScale = 1.0f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
