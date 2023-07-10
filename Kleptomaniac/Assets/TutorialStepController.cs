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
            tutorialPanel.SetActive(true);

            Time.timeScale = 0.0f;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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
