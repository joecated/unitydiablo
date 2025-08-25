// ShopUI.cs (CANVAS GROUP FÝNAL VERSÝYON)
using UnityEngine;
using TMPro;

public class ShopUI : MonoBehaviour
{
    public static ShopUI instance;
    public GameObject shopPanel;
    public Transform shopItemsContainer;
    public Transform playerItemsContainer;
    public GameObject shopSlotPrefab;
    public TextMeshProUGUI playerGoldText;

    private ShopData currentShopData; // Deðiþkenin türünü ShopData yaptýk
    private CanvasGroup canvasGroup;

    void Awake() { instance = this; canvasGroup = GetComponent<CanvasGroup>(); }
    void Start() { CloseShop(); }
    
    public void OpenShop(ShopData data)
    {
        currentShopData = data;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        RedrawPanels();
    }

    public void CloseShop()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void BuyItem(Item item)
    {
        if (PlayerStats.instance.SpendGold(item.price))
        {
            if (Inventory.instance.Add(item)) { RedrawPanels(); }
            else { PlayerStats.instance.AddGold(item.price); }
        }
    }
    public void RedrawPanels()
    {
        foreach (Transform child in shopItemsContainer) { Destroy(child.gameObject); }
        foreach (Transform child in playerItemsContainer) { Destroy(child.gameObject); }
        foreach (Item item in currentShopData.itemsForSale)
        {
            GameObject slotObj = Instantiate(shopSlotPrefab, shopItemsContainer);
            slotObj.GetComponent<ShopSlotUI>().Setup(item, true);
        }
        for (int i = 0; i < Inventory.instance.space; i++)
        {
            GameObject slotObj = Instantiate(shopSlotPrefab, playerItemsContainer);
            ShopSlotUI slotUI = slotObj.GetComponent<ShopSlotUI>();
            if (i < Inventory.instance.items.Count) { slotUI.Setup(Inventory.instance.items[i], false); }
            else { slotUI.ClearSlot(); }
        }
        playerGoldText.text = "Altýn: " + PlayerStats.instance.gold;
    }
}