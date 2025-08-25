// BlacksmithSlotUI.cs (F�YAT G�STERGES� EKLEND�)
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro kullanmak i�in bu sat�r �nemli

public class BlacksmithSlotUI : MonoBehaviour
{
    public Image icon;
    public Button button;
    public TextMeshProUGUI priceText; // YEN�: Fiyat metni i�in referans
    private Item item;

    public void Setup(Item newItem)
    {
        item = newItem;
        if (item != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
            button.interactable = true;

            // YEN�: Fiyat metnini doldur
            priceText.text = item.price.ToString();

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => BlacksmithUI.instance.OnPlayerItemClicked(item));
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        button.interactable = false;

        // YEN�: Slot temizlenince fiyat� da temizle
        priceText.text = "";
    }
}