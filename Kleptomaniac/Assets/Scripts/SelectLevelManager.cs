using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevelManager : MonoBehaviour
{
    #region Singleton Creation
    public static SelectLevelManager Instance { get; private set; }

    private SelectLevelManager()
    {
        // Private constructor to prevent instantiation from outside the class.
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }

        Instance = this;

        FillInventory();
        FillBackpack();
    }
    #endregion

    [SerializeField] int totalInventorySlot = 9;
    [SerializeField] int totalBackpackSlot = 3;

    [SerializeField] InventorySO inventory;
    [SerializeField] InventorySO backpack;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] GameObject inventoryGrid;
    [SerializeField] GameObject backpackGrid;

    [SerializeField] LevelDetailsController levelDetails;


    LevelDataSO.LevelCode _selectedLevel = LevelDataSO.LevelCode.NONE;

    public void FillInventory()
    {

        int counter = 0;

        inventory.items.ForEach(item => {
            GameObject newItemInstance = Instantiate(itemPrefab);
            GameObject newSlotInstance = Instantiate(slotPrefab);
            newSlotInstance.transform.SetParent(inventoryGrid.transform, false);
            newItemInstance.transform.SetParent(newSlotInstance.transform, false);

            if (!newItemInstance.TryGetComponent<InventoryItemController>(out var controller)) { return; }

            controller.SetItem(item.data);

            counter++;
        });

        if (counter < totalInventorySlot)
        {
            for(int i = 0; i < totalInventorySlot - counter;i++)
            {
                GameObject newSlotInstance = Instantiate(slotPrefab);
                newSlotInstance.transform.SetParent(inventoryGrid.transform, false);
            }
        }

      
    }

    public void FillBackpack()
    {

        int counter = 0;

        backpack.items.ForEach(item => {
            GameObject newItemInstance = Instantiate(itemPrefab);
            GameObject newSlotInstance = Instantiate(slotPrefab);
            newSlotInstance.transform.SetParent(backpackGrid.transform, false);
            newItemInstance.transform.SetParent(newSlotInstance.transform, false);

            if (!newItemInstance.TryGetComponent<InventoryItemController>(out var controller)) { return; }

            controller.SetItem(item.data);

            counter++;
        });

        if (counter < totalBackpackSlot)
        {
            for (int i = 0; i < totalBackpackSlot - counter; i++)
            {
                GameObject newSlotInstance = Instantiate(slotPrefab);
                newSlotInstance.transform.SetParent(backpackGrid.transform, false);
            }
        }


    }

    public void DisplayLevelInfo(LevelDataSO data)
    {
        levelDetails.ShowInfo(data);

        _selectedLevel = data.code;

    }

    public void MoveObjectToBackback(ItemSO item)
    {
        backpack.AddItem(item);
        inventory.RemoveItem(item);
    }

    public void MoveObjectToInventyory(ItemSO item)
    {
        inventory.AddItem(item);
        backpack.RemoveItem(item);
    }

    public void GoToBaclkmarket()
    {
        Debug.Log("Go to balckmarket");
    }

    public void GoToLevel()
    {

        switch (_selectedLevel)
        {
            case LevelDataSO.LevelCode.TUTORIAL:
                Debug.Log("Go to Level - Tutorial");
                break;
            case LevelDataSO.LevelCode.FIRST:
                Debug.Log("Go to Level - First");
                break;
            default:
                Debug.Log("Go to Level - NO LEVEL SELECTED");
                break;
        }
    }

    public void GoToExit()
    {
        Debug.Log("Go to Exit");
    }
}
