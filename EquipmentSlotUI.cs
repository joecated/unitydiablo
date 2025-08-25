using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // UI etkile�imlerini (t�klama gibi) dinlemek i�in bu sat�r gerekli.

// IPointerClickHandler: Bu aray�z, script'in fare t�klamalar�n� alg�lamas�n� sa�lar.
public class EquipmentSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    Equipment equipment;

    void Awake()
    {
        icon.enabled = false;
    }

    public void AddItem(Equipment newItem)
    {
        equipment = newItem;
        icon.sprite = equipment.icon;
        icon.enabled = true;
    }

    public void ClearSlot()
    {
        equipment = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    // --- YEN� EKLENEN FONKS�YON ---
    // Bu slota t�kland���nda Unity bu fonksiyonu otomatik olarak �al��t�r�r.
    public void OnPointerClick(PointerEventData eventData)
    {
        // E�er SA� fare tu�una t�kland�ysa...
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Ve bu slotun i�inde bir e�ya varsa...
            if (equipment != null)
            {
                // EquipmentManager'a bu slottaki e�yay� ��karmas�n� s�yle.
                EquipmentManager.instance.Unequip((int)equipment.equipSlot);
            }
        }
    }
    // Fare bu slotun �zerine geldi�inde tetiklenir
    public void OnPointerEnter(PointerEventData eventData)
    {
        // E�er bu slotta bir ekipman varsa...
        if (equipment != null)
        {
            // TooltipManager'a bu ekipman�n bilgilerini g�stermesini s�yle
            TooltipManager.instance.ShowTooltip(equipment);
        }
    }

    // Fare bu slotun �zerinden ayr�ld���nda tetiklenir
    public void OnPointerExit(PointerEventData eventData)
    {
        // TooltipManager'a bilgi ekran�n� gizlemesini s�yle
        TooltipManager.instance.HideTooltip();
    }



}