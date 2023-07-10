using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelGoals : MonoBehaviour
{
    public Image questimg1;
    public Image questimg2;
    public Image questimg3;

    // Start is called before the first frame update
    void Start()
    {
        questimg1 = transform.Find("Secondary Goal Panel (1)").GetComponent<Image>();
        questimg1.color = new Color(0.9176470588235294f, 0.4117647058823529f, 0.4117647058823529f);
        questimg2 = transform.Find("Secondary Goal Panel (2)").GetComponent<Image>();
        questimg2.color = new Color(0.6941176470588235f, 0.9725490196078431f, 0.3843137254901961f);
        questimg3 = transform.Find("Secondary Goal Panel (3)").GetComponent<Image>();
        questimg3.color = new Color(0.6941176470588235f, 0.9725490196078431f, 0.3843137254901961f);
    }

    // Update is called once per frame
    void Update()
    {
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
