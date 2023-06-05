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
    [SerializeField] Image itemImage;

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

        itemName.text = item.name;
        itemDesc.text = item.description;
        itemPrice.text = item.price + " €";
        itemImage.sprite = item.itemSprite;
    }

    public void HandleBuyItem()
    {
        Debug.Log("Compro oggetto e lo aggiungo a inventario personaggio");
    }
}
