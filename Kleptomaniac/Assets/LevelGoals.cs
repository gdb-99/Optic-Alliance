using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelGoals : MonoBehaviour
{
    public Image questimg1;
    public Image questimg2;
    public Image questimg3;

    //For the Tutorial
    [SerializeField] TextMeshProUGUI mainQuest;
    [SerializeField] TextMeshProUGUI secondQuest;
    //[SerializeField] GameObject questPanel1;
    [SerializeField] GameObject questPanel2;
    [SerializeField] GameObject questPanel3;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance.LevelData.hasSecondaryQuestion){
            questimg1 = transform.Find("Secondary Goal Panel (1)").GetComponent<Image>();
            questimg1.color = new Color(0.9176470588235294f, 0.4117647058823529f, 0.4117647058823529f);
            questimg2 = transform.Find("Secondary Goal Panel (2)").GetComponent<Image>();
            questimg2.color = new Color(0.6941176470588235f, 0.9725490196078431f, 0.3843137254901961f);
            questimg3 = transform.Find("Secondary Goal Panel (3)").GetComponent<Image>();
            questimg3.color = new Color(0.6941176470588235f, 0.9725490196078431f, 0.3843137254901961f);
        }
        else{

            //Gestisci il panel principale:
            mainQuest.text = "Steal the famous violin";

            //Gestisci i panel secondari:
            //questPanel1.SetActive(false);
            secondQuest.text = "Nothing else to do here";
            questPanel2.SetActive(false);
            questPanel3.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.LevelData.hasSecondaryQuestion){
            if(GameManager.Instance.quest1 == true)
            {
                questimg1.color = new Color(0.6941176470588235f, 0.9725490196078431f, 0.3843137254901961f);
            }
            if (GameManager.Instance.quest2 == false)
            {
                questimg2.color = new Color(0.9176470588235294f, 0.4117647058823529f, 0.4117647058823529f);
            }
            if (GameManager.Instance.quest3 == false)
            {
                questimg3.color = new Color(0.9176470588235294f, 0.4117647058823529f, 0.4117647058823529f);
            }
        }
    }
}
