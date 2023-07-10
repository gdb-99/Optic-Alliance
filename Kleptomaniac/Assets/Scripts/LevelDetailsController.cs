using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelDetailsController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI descText;
    [SerializeField] TextMeshProUGUI moneyRewardText;
    [SerializeField] TextMeshProUGUI reputationRewardText;
    [SerializeField] GameObject emptyPanel;
    [SerializeField] GameObject dataPanel;
    [SerializeField] GameObject donePanel;
    [SerializeField] GameObject errorPanel;



    // Start is called before the first frame update
    void Start()
    {
        donePanel.SetActive(false);
        emptyPanel.SetActive(true);
        dataPanel.SetActive(false);
        errorPanel.SetActive(false);
    }

    public void ShowInfo(LevelDataSO data)
    {
        emptyPanel.SetActive(false);
        dataPanel.SetActive(true);
        errorPanel.SetActive(false);

        donePanel.SetActive(data.done);

        descText.text = data.description;
        moneyRewardText.text = "+ " + data.profit.ToString();
        reputationRewardText.text = data.minReputationLevel.ToString();
    }

    public void ShowError()
    {
        donePanel.SetActive(false);
        emptyPanel.SetActive(false);
        dataPanel.SetActive(false);
        errorPanel.SetActive(true);
    }
}
