// BlacksmithSlotUI.cs (FÝYAT GÖSTERGESÝ EKLENDÝ)
using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro kullanmak için bu satýr önemli

public class BlacksmithSlotUI : MonoBehaviour
{
    public Image icon;
    public Button button;
    public TextMeshProUGUI priceText; // YENÝ: Fiyat metni için referans
    private Item item;

    public void Setup(Item newItem)
    {
        item = newItem;
        if (item != null)
        {
            icon.sprite = item.icon;
            icon.enabled = true;
            button.interactable = true;

            // YENÝ: Fiyat metnini doldur
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

        // YENÝ: Slot temizlenince fiyatý da temizle
        priceText.text = "";
    }
}