using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        playerItemController.OnItemEnteredCooldown += PlayerItemController_OnItemEnteredCooldown;
        playerItemController.OnItemCounterDecreased += PlayerItemController_OnItemCounterDecreased;

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
            //itemSlotTransform.Find("ItemNum").GetComponent<TextMeshProUGUI>().text = "( " + (i+1).ToString() + " )";
            itemSlotTransform.gameObject.SetActive(true);
            if(invetoryData.data.numberOfUses > 0) {
                Transform counter = itemSlotTransform.Find("Counter");
                counter.gameObject.SetActive(true);
                Debug.Log("Counter = " + counter);
                counter.GetComponent<TextMeshProUGUI>().text = invetoryData.data.numberOfUses + " / " + invetoryData.data.numberOfUses;
            }
            i++;
        }
    }

    private void PlayerItemController_OnItemCounterDecreased(object sender, PlayerItemController.OnItemCounterDecreasedEventArgs e) {
        Transform counter = slots[e.itemIndex].Find("Counter");
        counter.GetComponent<TextMeshProUGUI>().text = e.counterCurrent + " / " + e.counterMax;
    }

    private void PlayerItemController_OnItemEnteredCooldown(object sender, PlayerItemController.OnItemEnteredCooldownEventArgs e) {
        StartCoroutine(ItemEnteredCooldownCoroutine(e.coolDownTime, e.itemIndex));
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

    IEnumerator ItemEnteredCooldownCoroutine(float cooldownTime, int itemIndex) {

        float timer = 0f;
        Transform cooldownBar = slots[itemIndex].Find("CooldownBar");
        cooldownBar.gameObject.SetActive(true);
        Image barImage = cooldownBar.Find("Bar").GetComponent<Image>();
        barImage.fillAmount = 0f;


        while (timer < cooldownTime) {
            timer += Time.deltaTime;
            barImage.fillAmount = timer / cooldownTime;
            yield return null;
        }

        cooldownBar.gameObject.SetActive(false);
    }
}
