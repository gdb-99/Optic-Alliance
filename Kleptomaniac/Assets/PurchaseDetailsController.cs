using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShowItemDetailsEvent: UnityEvent<ItemSO> { }

public class PurchaseDetailsController : MonoBehaviour
{
    public ShowItemDetailsEvent showDetailsEvent;

    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDesc;
    [SerializeField] TextMeshProUGUI itemPrice;
    [SerializeField] TextMeshProUGUI itemType;
    [SerializeField] Image itemImage;
    [SerializeField] InventorySO globalInventory;

    ItemSO selectedItem;

    // Start is called before the first frame update
    void Start()
    {
        if (showDetailsEvent == null)
            showDetailsEvent = new ShowItemDetailsEvent();

        showDetailsEvent.AddListener(HandleShowDetails);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void HandleShowDetails(ItemSO item)
    {
        Debug.Log(item.name + " " + item.description);

        selectedItem = item;

        itemName.text = item.name;
        itemDesc.text = item.description;
        itemPrice.text = item.price + " $";
        itemType.text = item.isPermanent ? "PERMANENT" : "CONSUMABLE";
        itemImage.sprite = item.itemSprite;
    }

    public void HandleBuyItem()
    {
        globalInventory.AddItem(selectedItem);
        Debug.Log("Compro oggetto e lo aggiungo a inventario personaggio");
    }
}
