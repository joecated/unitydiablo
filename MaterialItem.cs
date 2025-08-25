// MaterialItem.cs (YEN� SCR�PT)
using UnityEngine;

[CreateAssetMenu(fileName = "New Material", menuName = "Inventory/Material")]
public class MaterialItem : Item
{
    // Bu script'in tek amac�, yeni materyal olu�tururken
    // Inspector'daki ayarlar� bizim i�in otomatik olarak yapmakt�r.
    void OnValidate()
    {
        itemType = ItemType.Material;
        isStackable = true;
        maxStackSize = 99; // Diledi�in gibi de�i�tirebilirsin
    }
}