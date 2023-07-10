using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDetailsController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI detailsText;
    public void ShowInfo(ItemSO item) {

        if(item.isPermanent){
            detailsText.text = "PERMANENT ITEM.\n";
        }
        else{
            detailsText.text = "CONSUMSBLE ITEM.\n";
        }

        detailsText.text += item.description;
    }
}
