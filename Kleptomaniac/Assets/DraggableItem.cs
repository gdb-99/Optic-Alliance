using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    
    [HideInInspector] public Transform parentAfterDrag;

    RectTransform _rectTrans;
    Image _image;
    string _startFrom;


    private void Start()
    {
        _rectTrans = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        _startFrom = transform.parent.tag;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        _image.raycastTarget = false;

        //Tag is Inventory or Backpack
        
        Debug.Log("BEGIN DRAG " + _startFrom);


    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _rectTrans.anchoredPosition3D = new(_rectTrans.anchoredPosition3D.x, _rectTrans.anchoredPosition3D.y, 0f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        _image.raycastTarget = true;

        Debug.Log("END DRAG " + _startFrom + " " + parentAfterDrag.tag);

        if(parentAfterDrag.tag == "SlotInventory" || parentAfterDrag.tag == "SlotBackpack")
        {
            if(_startFrom != parentAfterDrag.tag)
            {
                if(parentAfterDrag.tag == "SlotBackpack")
                {

                    SelectLevelManager.Instance.MoveObjectToBackback(GetComponent<InventoryItemController>().GetItem());
                }
                else SelectLevelManager.Instance.MoveObjectToInventyory(GetComponent<InventoryItemController>().GetItem());
                Debug.Log("Sposto oggetto");

                SelectLevelManager.Instance.LoadItemsInventory();

            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectLevelManager.Instance.DisplayItemInfo(GetComponent<InventoryItemController>().GetItem());
    }
}
