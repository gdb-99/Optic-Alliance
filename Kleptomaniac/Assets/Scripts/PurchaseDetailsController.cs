using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShowItemDetailsEvent: UnityEvent<ItemSO> { }

public class PurchaseDetailsController : MonoBehaviour
{
    public ShowItemDetailsEvent showDetailsEvent;

    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI itemDesc;
    [SerializeField] TextMeshProUGUI itemPrice;
    [SerializeField] TextMeshProUGUI itemType;
    [SerializeField] TextMeshProUGUI emptyText;
    [SerializeField] Image itemImage;
    [SerializeField] PlayerSO player;
    [SerializeField] Button buyButton;
    [SerializeField] TextMeshProUGUI conversation;
    [SerializeField] TextMeshProUGUI money;
    [SerializeField] TextMeshProUGUI reputation;
    [SerializeField] TextMeshProUGUI itemReputation;
    [SerializeField] Image itemPriceImage;
    [SerializeField] Image itemReputationImage;
    [SerializeField] Button yesButton, noButton;

    ItemSO selectedItem;

    // Start is called before the first frame update
    void Start()
    {
        if (showDetailsEvent == null)
            showDetailsEvent = new ShowItemDetailsEvent();
        showDetailsEvent.AddListener(HandleShowDetails);
        money.text = player.money.ToString();
        reputation.text = player.reputation.ToString();
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Reset()
    {
        itemName.text = string.Empty;
        itemDesc.text = string.Empty;
        itemPrice.text = string.Empty;
        itemType.text = string.Empty;
        itemReputation.text = string.Empty;
        itemImage.sprite = null;
        itemImage.enabled = false;
        itemPriceImage.enabled = false;
        itemReputationImage.enabled = false;

        emptyText.enabled = true;

    }

    void HandleShowDetails(ItemSO item)
    {
        selectedItem = item;

        itemName.text = item.name;
        itemDesc.text = item.description;
        itemPrice.text = item.price.ToString();
        itemType.text = item.isPermanent ? "PERMANENT" : "CONSUMABLE";
        itemImage.sprite = item.itemSprite;
        itemImage.enabled = true;
        itemPriceImage.enabled = true;
        itemReputationImage.enabled = true;
        itemReputation.text = item.reputation.ToString();
        SetBuyButtonInteractable();
        emptyText.enabled = false;
    }

    public void SetBuyButtonInteractable() {
        if (selectedItem.isPermanent && player.globalInv.items.Exists((i) => i.data == selectedItem)) {
            buyButton.interactable = false;
            conversation.text = "You've already bought one! Another one would be useless!";
        }
        else if(player.money < selectedItem.price) {
            buyButton.interactable = false;
            conversation.text = "You don't have enough money! Try something cheaper!";
        }
        else if (player.reputation < selectedItem.reputation) {
            buyButton.interactable = false;
            conversation.text = "You don't have enough reputation! Come back some other time!";
        }
        else {
            buyButton.interactable = true;
            conversation.text = "Do you like it? It's perfect for you!";
        }
    }

    public void BuyItemOnClick() {
        buyButton.interactable = false;
        conversation.text = "Do you really want to buy that item?";
        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);
    } 

    public void DontBuyItem() {
        SetBuyButtonInteractable();
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        conversation.text = "Please look around, you'll find something useful!";
    }

    public void BuyItem()
    {
        player.globalInv.AddItem(selectedItem);
        player.SubstractMoney((int)selectedItem.price);
        money.text = player.money.ToString();
        SetBuyButtonInteractable();
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        conversation.text = "Thank you for your purchase!"; 
    }

    public void NavigateTo(string scene) {
        SceneManager.LoadScene(scene);
    }
}