using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionCompletedUIController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelName;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI reputationText;
    [SerializeField] Image star1Img;
    [SerializeField] Image star2Img;
    [SerializeField] Image star3Img;
    [SerializeField] Sprite star;
    [SerializeField] Sprite emptyStar;

    public void ShowVictory(int completedQuest)
    {

        LevelDataSO data = GameManager.Instance.LevelData;

        if (data.hasSecondaryQuestion)
        {
            if (completedQuest == 3)
            {
                star1Img.sprite = star;
                star2Img.sprite = star;
                star3Img.sprite = star;
            }
            else if (completedQuest == 2)
            {
                star1Img.sprite = star;
                star2Img.sprite = star;
                star3Img.sprite = emptyStar;
            }
            else if (completedQuest == 1)
            {
                star1Img.sprite = star;
                star2Img.sprite = emptyStar;
                star3Img.sprite = emptyStar;
            }
            else
            {
                star1Img.sprite = emptyStar;
                star2Img.sprite = emptyStar;
                star3Img.sprite = emptyStar;
            }
        }
        else
        {
            star1Img.gameObject.SetActive(false);
            star2Img.gameObject.SetActive(false);
            star3Img.gameObject.SetActive(false);
            completedQuest = 0;
        }
       

        levelName.text = data.name + " completed!";
        moneyText.text = data.profit.ToString() + (completedQuest > 0 ? " | +"+(10 * completedQuest) : "");
        reputationText.text = "1";

        gameObject.SetActive(true);

    }

}
