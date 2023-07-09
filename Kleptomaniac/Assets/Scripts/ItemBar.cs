using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBar : MonoBehaviour {
    [SerializeField] private InventorySO backpack;

    private PlayerItemController playerItemController;

    private Transform itemSlotTemplate;
    private Color notSelected = new Color(0.9254902f, 0.6235294f, 0.0196078f);
    private Color selected = new Color(0.7490196f, 0.1921569f, 0f);

    private List<Transform> slots = new List<Transform>();

    private void Awake() {
        itemSlotTemplate = transform.Find("ItemSlotTemplate");
        itemSlotTemplate.gameObject.SetActive(false);
    }

    void Start() {
        playerItemController = Player.Instance.GetComponent<PlayerItemController>();
        playerItemController.OnItemSwitched += PlayerItemController_OnItemSwitched;

        int i = 0;
        foreach (InvetoryData invetoryData in backpack.items) {
            //Debug.Log(backpack.items2[i].name);
            Transform itemSlotTransform = Instantiate(itemSlotTemplate, transform);
            slots.Add(itemSlotTransform);

            if(playerItemController.GetCurrentItemIndex() == i) {
                itemSlotTransform.GetComponent<Image>().color = selected;
            } else {
                itemSlotTransform.GetComponent<Image>().color = notSelected;
            }

            RectTransform itemSlotRectTransform = itemSlotTransform.GetComponent<RectTransform>();
            itemSlotRectTransform.anchoredPosition = new Vector2(50f * i, 0f);
            itemSlotTransform.Find("ItemImage").GetComponent<Image>().sprite = backpack.items[i].data.itemSprite;
            itemSlotTransform.gameObject.SetActive(true);
            i++;
        }
    }

    private void PlayerItemController_OnItemSwitched(object sender, System.EventArgs e) {
        int i = 0;
        foreach(Transform slot in slots) {
            if(playerItemController.GetCurrentItemIndex() == i) {
                slot.GetComponent<Image>().color = selected;
            } else {
                slot.GetComponent<Image>().color = notSelected;
            }
            i++;
        }
    }
}
