using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBar : MonoBehaviour {
    [SerializeField] private InventorySO backpack;

    private Transform itemSlotTemplate;

    private void Awake() {
        itemSlotTemplate = transform.Find("ItemSlotTemplate");
        itemSlotTemplate.gameObject.SetActive(false);
    }

    void Start() {
        int i = 0;
        foreach (InvetoryData invetoryData in backpack.items) {
            //Debug.Log(backpack.items2[i].name);
            Transform itemSlotTransform = Instantiate(itemSlotTemplate, transform);
            RectTransform itemSlotRectTransform = itemSlotTransform.GetComponent<RectTransform>();
            Debug.Log("ITEM RECT TRANSFORM = " + itemSlotRectTransform);
            itemSlotRectTransform.anchoredPosition = new Vector2(50f * i, 0f);
            itemSlotTransform.Find("ItemImage").GetComponent<Image>().sprite = backpack.items[i].data.itemSprite;
            itemSlotTransform.gameObject.SetActive(true);
            i++;
        }
    }

}
