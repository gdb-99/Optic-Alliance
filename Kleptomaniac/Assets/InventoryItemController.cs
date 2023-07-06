using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{

    [SerializeField] ItemSO item;
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

    public void SetItem(ItemSO itemToSet)
    {
        item = itemToSet;
        _image = GetComponent<Image>();
        _image.sprite = itemToSet.itemSprite;

        //_detailsPanel = FindObjectOfType<PurchaseDetailsController>();
    }
}
