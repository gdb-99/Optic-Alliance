using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        LoadItemsInventory();
        SetPlayerData();
    }
    #endregion

    [SerializeField] Button startButton;

    [SerializeField] PlayerSO playerData;

    [SerializeField] int totalInventorySlot = 9;
    [SerializeField] int totalBackpackSlot = 3;

    //[SerializeField] InventorySO inventory;
    //[SerializeField] InventorySO backpack;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] GameObject inventoryGrid;
    [SerializeField] GameObject backpackGrid;

    [SerializeField] LevelDetailsController levelDetails;
    [SerializeField] ItemDetailsController itemDetails;

    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] TextMeshProUGUI reputationText;


    LevelDataSO _selectedLevel;
    
    void Start()
    {
        startButton.enabled = false;
    }

    void Update(){
        if(_selectedLevel != null){
            startButton.enabled = true;
        }
    }

    void SetPlayerData()
    {
        moneyText.text = playerData.money.ToString();
        reputationText.text = playerData.reputation.ToString();
    }

    void FillInventory()
    {

        int counter = 0;

        foreach (Transform child in inventoryGrid.transform)
        {
            Destroy(child.gameObject);
        }

        playerData.globalInv.items.ForEach(item => {
            GameObject newItemInstance = Instantiate(itemPrefab);
            GameObject newSlotInstance = Instantiate(slotPrefab);
            newSlotInstance.transform.SetParent(inventoryGrid.transform, false);
            newSlotInstance.tag = "SlotInventory";
            newItemInstance.transform.SetParent(newSlotInstance.transform, false);

            if (!newItemInstance.TryGetComponent<InventoryItemController>(out var controller)) { return; }

            controller.SetItem(item);

            counter++;
        });

        if (counter < totalInventorySlot)
        {
            for(int i = 0; i < totalInventorySlot - counter;i++)
            {
                GameObject newSlotInstance = Instantiate(slotPrefab);
                newSlotInstance.tag = "SlotInventory";
                newSlotInstance.transform.SetParent(inventoryGrid.transform, false);
            }
        }

      
    }

    void FillBackpack()
    {

        int counter = 0;

        foreach (Transform child in backpackGrid.transform)
        {
            Destroy(child.gameObject);
        }

        playerData.backpackbackInv.items.ForEach(item => {
            GameObject newItemInstance = Instantiate(itemPrefab);
            GameObject newSlotInstance = Instantiate(slotPrefab);
            newSlotInstance.transform.SetParent(backpackGrid.transform, false);
            newSlotInstance.tag = "SlotBackpack";
            newItemInstance.transform.SetParent(newSlotInstance.transform, false);

            if (!newItemInstance.TryGetComponent<InventoryItemController>(out var controller)) { return; }

            controller.SetItem(item);

            counter++;
        });

        if (counter < totalBackpackSlot)
        {
            for (int i = 0; i < totalBackpackSlot - counter; i++)
            {
                GameObject newSlotInstance = Instantiate(slotPrefab);
                newSlotInstance.tag = "SlotBackpack";
                newSlotInstance.transform.SetParent(backpackGrid.transform, false);
            }
        }


    }

    public void LoadItemsInventory()
    {
        FillBackpack();
        FillInventory();
    }

    public void DisplayLevelInfo(LevelDataSO data)
    {
        levelDetails.ShowInfo(data);
        _selectedLevel = data;

    }

    public void DisplayItemInfo(ItemSO data)
    {
        itemDetails.ShowInfo(data);
    }

    public void MoveObjectToBackback(ItemSO item)
    {
        playerData.backpackbackInv.AddItem(item);
        playerData.globalInv.RemoveItem(item);
    }

    public void MoveObjectToInventyory(ItemSO item)
    {
        playerData.globalInv.AddItem(item);
        playerData.backpackbackInv.RemoveItem(item);
    }

    public void GoToBaclkmarket()
    {
        Debug.Log("Go to balckmarket");
        SceneManager.LoadScene("BlackMarketScene");
    }

    public void GoToLevel()
    {

        if (_selectedLevel.minReputationLevel > playerData.reputation)
        {
            levelDetails.ShowError();
        }
        else
        {
            switch (_selectedLevel.code)
            {
                case LevelDataSO.LevelCode.TUTORIAL:
                    Debug.Log("Go to Level - Tutorial");
                    SceneManager.LoadScene("TutorialScene");
                    break;
                case LevelDataSO.LevelCode.FIRST:
                    Debug.Log("Go to Level - First");
                    SceneManager.LoadScene("FirstLevelScene");
                    break;
                default:
                    Debug.Log("Go to Level - NO LEVEL SELECTED");
                    break;
            }
        }

        
    }

    public void GoToExit()
    {
        SceneManager.LoadScene("MainMenuScene");
    }




}
