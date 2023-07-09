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



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowInfo(LevelDataSO data)
    {
        emptyPanel.SetActive(false);
        dataPanel.SetActive(true);

        descText.text = data.description;
        moneyRewardText.text = "+ " + data.profit.ToString();
        reputationRewardText.text = data.minReputationLevel.ToString();
    }
}
