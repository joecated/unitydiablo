// MaterialItem.cs (YENÝ SCRÝPT)
using UnityEngine;

[CreateAssetMenu(fileName = "New Material", menuName = "Inventory/Material")]
public class MaterialItem : Item
{
    // Bu script'in tek amacý, yeni materyal oluþtururken
    // Inspector'daki ayarlarý bizim için otomatik olarak yapmaktýr.
    void OnValidate()
    {
        itemType = ItemType.Material;
        isStackable = true;
        maxStackSize = 99; // Dilediðin gibi deðiþtirebilirsin
    }
}