using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // UI etkileþimlerini (týklama gibi) dinlemek için bu satýr gerekli.

// IPointerClickHandler: Bu arayüz, script'in fare týklamalarýný algýlamasýný saðlar.
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

    // --- YENÝ EKLENEN FONKSÝYON ---
    // Bu slota týklandýðýnda Unity bu fonksiyonu otomatik olarak çalýþtýrýr.
    public void OnPointerClick(PointerEventData eventData)
    {
        // Eðer SAÐ fare tuþuna týklandýysa...
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // Ve bu slotun içinde bir eþya varsa...
            if (equipment != null)
            {
                // EquipmentManager'a bu slottaki eþyayý çýkarmasýný söyle.
                EquipmentManager.instance.Unequip((int)equipment.equipSlot);
            }
        }
    }
    // Fare bu slotun üzerine geldiðinde tetiklenir
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Eðer bu slotta bir ekipman varsa...
        if (equipment != null)
        {
            // TooltipManager'a bu ekipmanýn bilgilerini göstermesini söyle
            TooltipManager.instance.ShowTooltip(equipment);
        }
    }

    // Fare bu slotun üzerinden ayrýldýðýnda tetiklenir
    public void OnPointerExit(PointerEventData eventData)
    {
        // TooltipManager'a bilgi ekranýný gizlemesini söyle
        TooltipManager.instance.HideTooltip();
    }



}