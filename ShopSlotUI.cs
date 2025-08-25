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
            // E�er bu d�kkan slotu ise, t�kland���nda "BuyItem" fonksiyonunu �a��r
            button.onClick.AddListener(BuyItem);
        }
        else
        {
            // E�er oyuncu envanter slotu ise, t�kland���nda "SellItem" fonksiyonunu �a��r
            // �imdilik satma i�levini eklemiyoruz, onu sonra yapabiliriz.
        }
    }
    // YEN� FONKS�YON: Bu slotu bo� bir slota d�n��t�r�r.
    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        priceText.text = ""; // Fiyat� da temizle
        button.onClick.RemoveAllListeners();
        button.interactable = false; // Butonu t�klanmaz yap
    }
    void BuyItem()
    {
        ShopUI.instance.BuyItem(item);
    }
}