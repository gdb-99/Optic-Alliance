using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    //Instance:
    #region Singleton Creation
    public static PauseMenu Instance { get; private set; }

    private PauseMenu()
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
    }
    #endregion

    //Per mettere in pausa il gioco e mostrare il corretto panel:
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    public ShowItemDetailsEvent showDetailsEvent;
    [SerializeField] GameObject _prefabItem;
    [SerializeField] GameObject slotPrefab;
    [SerializeField] PlayerSO player;
    [SerializeField] VerticalLayoutGroup itemList;

    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDesc;
    [SerializeField] TextMeshProUGUI emptyText;
    [SerializeField] Image itemImage;

    [SerializeField] GameObject itemPanel;
    
    ItemSO selectedItem;

    /*
    void Start() {
        Resume();
    }
    */
  
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(pauseMenuUI.activeSelf)
            {
                Resume();
            }
            else 
            {
                if(!GameIsPaused){
                    Pause();
                }
            }
        }

    }

    public void Pause()
    {
        //Mostra il cursore del mouse:
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseMenuUI.SetActive(true);    //abilita la visualizzazione del panel della pausa
        Time.timeScale = 0f;    //per bloccare il gioco
        GameIsPaused = true;
    }

    public void Resume()
    {
        //Nascondi il cursore del mouse:
        Debug.Log("CALLED RESUME FUNCTION!");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Resume();
    }

    public void QuitLevel() {
        SceneManager.LoadScene("SelectLevelScene");
        Resume();
    }

    private void ResetShowPanel()
    {
        itemName.text = string.Empty;
        itemDesc.text = string.Empty;
        itemImage.sprite = null;
        itemImage.enabled = false;
        emptyText.enabled = true;
    }

    public void HandleShowDetails(ItemSO item)
    {
        //selectedItem = item;
        Debug.Log("Item:" + item.name);

        itemName.text = item.name;
        itemDesc.text = item.description;
        itemImage.sprite = item.itemSprite;
        itemPanel.SetActive(true);
        itemImage.enabled = true;
        emptyText.enabled = false;
    }

    public void MyObjects() {

        FillBackpack();
        ResetShowPanel();
        
    }

    public void FillBackpack()
    {

        int counter = 0;

        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }

        player.backpackbackInv.items.ForEach(item => {
            GameObject newItemInstance = Instantiate(_prefabItem);
            GameObject newSlotInstance = Instantiate(slotPrefab);
            newSlotInstance.transform.SetParent(itemList.transform, false);
            newSlotInstance.tag = "SlotBackpack";
            newItemInstance.transform.SetParent(newSlotInstance.transform, false);

            if (!newItemInstance.TryGetComponent<InventoryItemController>(out var controller)) 
            { 
                return; 
            }

            controller.SetItem(item);

            counter++;
        });

        if (counter < 3)
        {
            for (int i = 0; i < 3 - counter; i++)
            {
                GameObject newSlotInstance = Instantiate(slotPrefab);
                newSlotInstance.tag = "SlotBackpack";
                newSlotInstance.transform.SetParent(itemList.transform, false);
            }
        }
    }
}
