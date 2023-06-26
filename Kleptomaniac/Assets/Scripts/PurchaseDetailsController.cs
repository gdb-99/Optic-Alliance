using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ShowItemDetailsEvent: UnityEvent<ItemSO> { }

public class PurchaseDetailsController : MonoBehaviour
{
    public ShowItemDetailsEvent showDetailsEvent;

    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDesc;
    [SerializeField] TextMeshProUGUI itemPrice;
    [SerializeField] TextMeshProUGUI itemType;
    [SerializeField] TextMeshProUGUI emptyText;
    [SerializeField] Image itemImage;
    [SerializeField] InventorySO globalInventory;

    ItemSO selectedItem;

    // Start is called before the first frame update
    void Start()
    {
        if (showDetailsEvent == null)
            showDetailsEvent = new ShowItemDetailsEvent();

        showDetailsEvent.AddListener(HandleShowDetails);

        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Reset()
    {
        itemName.text = string.Empty;
        itemDesc.text = string.Empty;
        itemPrice.text = string.Empty;
        itemType.text = string.Empty;
        itemImage.sprite = null;
        itemImage.enabled = false;

        emptyText.enabled = true;

    }

    void HandleShowDetails(ItemSO item)
    {
        selectedItem = item;

        itemName.text = item.name;
        itemDesc.text = item.description;
        itemPrice.text = item.price + " $";
        itemType.text = item.isPermanent ? "PERMANENT" : "CONSUMABLE";
        itemImage.sprite = item.itemSprite;
        itemImage.enabled = true;

        emptyText.enabled = false;
    }

    public void HandleBuyItem()
    {
        globalInventory.AddItem(selectedItem);
        Debug.Log("Compro oggetto e lo aggiungo a inventario personaggio");
    }
}
