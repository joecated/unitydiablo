// Item.cs (TÜR VE BÝRÝKTÝRME ÖZELLÝÐÝ EKLENDÝ)
using UnityEngine;

// Bu enum'ý class'ýn dýþýna yazýyoruz ki diðer script'ler de kolayca eriþebilsin.
public enum ItemType { Equipment, Consumable, Material }

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public int price = 10; // YENÝ: Eþyanýn altýn deðeri

    [Header("Item Bilgileri")]
    // Inspector'da daha geniþ bir metin alaný saðlar
    [TextArea(3, 5)]
    public string description; // YENÝ: "Demir kýlýç yapýmýnda kullanýlýr." gibi bir açýklama

    [TextArea(2, 4)]
    public string source;      // YENÝ: "Dað Golemlerinden düþer." gibi bir kaynak bilgisi

    [Header("Item Ayarlarý")]
    public ItemType itemType; // YENÝ: Eþyanýn türü (Ekipman, Materyal vb.)
    public bool isStackable = false; // YENÝ: Bu eþya üst üste birikebilir mi?
    public int maxStackSize = 1; // YENÝ: Birikebilirse, en fazla kaç tane olabilir?

    public virtual void Use()
    {
        Debug.Log(name + " kullanýldý.");
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}