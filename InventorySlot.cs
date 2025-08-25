// InventorySlot.cs (SLOT KAYBOLMA HATASI ÝÇÝN SON DÜZELTME)
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler , IPointerEnterHandler, IPointerExitHandler
{
    // Bu, artýk slotun altýndaki 'Icon' objesine atanmalýdýr.
    // Ana objenin Image'ý ise arka plan olarak kalýr.
    public Image icon;
    Item item;

    void Awake()
    {
        // Baþlangýçta ikonun kapalý olduðundan emin ol
        if (icon != null)
        {
            icon.enabled = false;
        }
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        // Ýkonu görünür yap.
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        // Ýkonu kapat. Ana obje (arka plan) ve Layout Element etkilenmez.
        icon.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && item != null)
        {
            item.Use();
        }
    }

    // --- YENÝ EKLENEN FONKSÝYONLAR ---

    public void OnPointerEnter(PointerEventData eventData)
    {
        // --- BU SATIRI TEST ÝÇÝN EKLE ---
        Debug.Log("Fare þu slotun üzerine geldi: " + gameObject.name);
        // ------------------------------------

        if (item != null)
        {
            TooltipManager.instance.ShowTooltip(item);
        }
    }

    // Fare slotun üzerinden ayrýldýðýnda...
    public void OnPointerExit(PointerEventData eventData)
    {
        // TooltipManager'a tooltip'i gizlemesini söyle
        TooltipManager.instance.HideTooltip();
    }
}
