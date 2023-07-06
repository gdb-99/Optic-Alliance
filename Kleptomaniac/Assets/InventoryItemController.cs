using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{

    [SerializeField] ItemSO item;
    [SerializeField] TextMeshProUGUI couterText;
    [SerializeField] GameObject counterContainer;
    Image _image;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ItemSO GetItem()
    {
        return item;
    }

    public void SetItem(InvetoryData inventory)
    {
        item = inventory.data;
        _image = GetComponent<Image>();
        _image.sprite = inventory.data.itemSprite;
        couterText.text = inventory.counter.ToString();

        counterContainer.SetActive(inventory.counter > 1);


        //_detailsPanel = FindObjectOfType<PurchaseDetailsController>();
    }
}
