using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuItemController : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] ItemSO item;
    Image _image;
    PurchaseDetailsController _detailsPanel;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Mouse click");
        _detailsPanel.showDetailsEvent.Invoke(item);


    }

    public void SetItem(ItemSO itemToSet)
    {
        item = itemToSet;
        _image = GetComponent<Image>();
        _image.sprite = itemToSet.itemSprite;

        _detailsPanel = FindObjectOfType<PurchaseDetailsController>();
    }
}
