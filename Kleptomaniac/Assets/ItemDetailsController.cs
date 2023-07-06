using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemDetailsController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI detailsText;
    public void ShowInfo(ItemSO item) {
        detailsText.text = item.description;
    }
}
