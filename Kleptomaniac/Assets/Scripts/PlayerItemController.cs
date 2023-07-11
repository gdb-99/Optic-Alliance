using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemController : MonoBehaviour {

    public event EventHandler OnItemSwitched;
    public event EventHandler<OnItemEnteredCooldownEventArgs> OnItemEnteredCooldown;
    public event EventHandler<OnItemCounterDecreasedEventArgs> OnItemCounterDecreased;

    public class OnItemEnteredCooldownEventArgs : EventArgs {
        public int itemIndex;
        public float coolDownTime;
    }

    public class OnItemCounterDecreasedEventArgs : EventArgs {
        public int itemIndex;
        public int counterMax;
        public int counterCurrent;
    }

    [SerializeField] private InventorySO inventorySO;
    [SerializeField] private Transform itemHoldPoint;

    private List<Item> availableItems = new List<Item>();
    private int currentItemIndex = 0;

    private void Start() {
        //TODO take models from inventory and instantiate as inactive
        int i = 0;
        //foreach(InvetoryData invetoryData in inventorySO.items) {
        //    GameObject item = Instantiate(invetoryData.data.itemModel, itemHoldPoint);
        //    availableItems[i++] = item.GetComponent<Item>();
        //}
        foreach (InvetoryData invetoryData in inventorySO.items) {
            ItemSO itemSO = invetoryData.data;
            GameObject item = Instantiate(itemSO.itemModel, itemHoldPoint);
            availableItems.Add(item.GetComponent<Item>());
            Debug.Log("ITEM = " + availableItems[i]);
            availableItems[i].SetPlayerItemController(this);
            if (i == 0) {
                item.SetActive(true);
            } else {
                item.SetActive(false);
            }
            i++;
        }
    }

    public void SwitchActiveItem(int itemIndex) {
        if (itemIndex > availableItems.Count)
            return;

        foreach (Item item in availableItems) {
            item.gameObject.SetActive(false);
        }
        availableItems[itemIndex - 1].gameObject.SetActive(true);
        currentItemIndex = itemIndex - 1;
        OnItemSwitched?.Invoke(this, EventArgs.Empty);
    }

    public void UseActiveItem() {
        if(availableItems.Count > 0)
            availableItems[currentItemIndex].Use();
    }

    public void DropActiveItem() {
        Debug.Log("RESETTING AN ITEM");
        //GameObject item = Instantiate(inventorySO.items2[currentItemIndex].itemModel, itemHoldPoint);
        GameObject item = Instantiate(inventorySO.items[currentItemIndex].data.itemModel, itemHoldPoint);
        availableItems[currentItemIndex] = item.GetComponent<Item>();
        availableItems[currentItemIndex].SetPlayerItemController(this);
    }

    public int GetCurrentItemIndex() {
        return currentItemIndex;
    }

    public void NotifyItemCounterDecreased(int current, int max) {
        OnItemCounterDecreased?.Invoke(this, new OnItemCounterDecreasedEventArgs {
            itemIndex = currentItemIndex,
            counterCurrent = current,
            counterMax = max
        });
    }

    public void StartCooldown(float cooldownTime, Action f) {
        OnItemEnteredCooldown?.Invoke(this, new OnItemEnteredCooldownEventArgs {
            itemIndex = currentItemIndex,
            coolDownTime = cooldownTime
        });
        StartCoroutine(CoolDown(cooldownTime, f));
    }
    IEnumerator CoolDown(float cooldownTime, Action f) {
        Debug.Log("ITEM IN COOLDOWN");
        yield return new WaitForSeconds(cooldownTime);
        Debug.Log("ITEM FINISHED COOLDOWN");
        f();
    }

}