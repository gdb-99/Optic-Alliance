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
        _image= GetComponent<Image>();
        _image.sprite = item.itemSprite;

        _detailsPanel = FindObjectOfType<PurchaseDetailsController>();
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
}
