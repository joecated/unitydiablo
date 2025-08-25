using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI priceText;
    public Button button;
    private Item item;

    public void Setup(Item newItem, bool isShopItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;
        priceText.text = item.price.ToString();
        button.onClick.RemoveAllListeners();

        if (isShopItem)
        {
            // Eðer bu dükkan slotu ise, týklandýðýnda "BuyItem" fonksiyonunu çaðýr
            button.onClick.AddListener(BuyItem);
        }
        else
        {
            // Eðer oyuncu envanter slotu ise, týklandýðýnda "SellItem" fonksiyonunu çaðýr
            // Þimdilik satma iþlevini eklemiyoruz, onu sonra yapabiliriz.
        }
    }
    // YENÝ FONKSÝYON: Bu slotu boþ bir slota dönüþtürür.
    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        priceText.text = ""; // Fiyatý da temizle
        button.onClick.RemoveAllListeners();
        button.interactable = false; // Butonu týklanmaz yap
    }
    void BuyItem()
    {
        ShopUI.instance.BuyItem(item);
    }
}