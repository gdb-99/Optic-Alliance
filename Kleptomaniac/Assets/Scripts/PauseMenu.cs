using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public ShowItemDetailsEvent showDetailsEvent;
    public GameObject pauseMenuUI;
    [SerializeField] GameObject _prefabItem;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDesc;
    [SerializeField] TextMeshProUGUI emptyText;
    [SerializeField] Image itemImage;
    [SerializeField] PlayerSO player;
    [SerializeField] VerticalLayoutGroup itemList;
    ItemSO selectedItem;
 
    

    void Start() {
       
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(GameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }

        

        void Pause()
        {

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            pauseMenuUI.SetActive(true);    //abilita la visualizzazione del panel della pausa
            Time.timeScale = 0f;    //per bloccare il gioco
            GameIsPaused = true;
        }
    }

    public void Resume()
    {

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
    }
    
    /*
    public void fillBackpack() {
        item1 = player.backpackbackInv.items[0].data;
        item2 = player.backpackbackInv.items[1].data;
    }
    */

    private void ResetShowPanel()
    {
        itemName.text = string.Empty;
        itemDesc.text = string.Empty;
        itemImage.sprite = null;
        itemImage.enabled = false;

        emptyText.enabled = true;
    }

    void HandleShowDetails(ItemSO item)
    {
        selectedItem = item;

        itemName.text = item.name;
        itemDesc.text = item.description;
        itemImage.sprite = item.itemSprite;
        itemImage.enabled = true;
        emptyText.enabled = false;
    }

    public void MyObjects() {
       //FillBackpack();
        if (showDetailsEvent == null)
            showDetailsEvent = new ShowItemDetailsEvent();

        showDetailsEvent.AddListener(HandleShowDetails);
        ResetShowPanel();
    }

    public void FillBackpack()
    {
        player.backpackbackInv.items.ForEach(item => {
            GameObject newItemInstance = Instantiate(_prefabItem);
            newItemInstance.transform.SetParent(itemList.transform, false);

            if(!newItemInstance.TryGetComponent<MenuItemController>(out var controller)) { return; }

            controller.SetItem(item.data);
        });
    }
}
