// InventorySlot.cs (SLOT KAYBOLMA HATASI ���N SON D�ZELTME)
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler , IPointerEnterHandler, IPointerExitHandler
{
    // Bu, art�k slotun alt�ndaki 'Icon' objesine atanmal�d�r.
    // Ana objenin Image'� ise arka plan olarak kal�r.
    public Image icon;
    Item item;

    void Awake()
    {
        // Ba�lang��ta ikonun kapal� oldu�undan emin ol
        if (icon != null)
        {
            icon.enabled = false;
        }
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        // �konu g�r�n�r yap.
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        // �konu kapat. Ana obje (arka plan) ve Layout Element etkilenmez.
        icon.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && item != null)
        {
            item.Use();
        }
    }

    // --- YEN� EKLENEN FONKS�YONLAR ---

    public void OnPointerEnter(PointerEventData eventData)
    {
        // --- BU SATIRI TEST ���N EKLE ---
        Debug.Log("Fare �u slotun �zerine geldi: " + gameObject.name);
        // ------------------------------------

        if (item != null)
        {
            TooltipManager.instance.ShowTooltip(item);
        }
    }

    // Fare slotun �zerinden ayr�ld���nda...
    public void OnPointerExit(PointerEventData eventData)
    {
        // TooltipManager'a tooltip'i gizlemesini s�yle
        TooltipManager.instance.HideTooltip();
    }
}
