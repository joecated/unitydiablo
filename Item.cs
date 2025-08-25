// Item.cs (T�R VE B�R�KT�RME �ZELL��� EKLEND�)
using UnityEngine;

// Bu enum'� class'�n d���na yaz�yoruz ki di�er script'ler de kolayca eri�ebilsin.
public enum ItemType { Equipment, Consumable, Material }

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    public Sprite icon = null;
    public int price = 10; // YEN�: E�yan�n alt�n de�eri

    [Header("Item Bilgileri")]
    // Inspector'da daha geni� bir metin alan� sa�lar
    [TextArea(3, 5)]
    public string description; // YEN�: "Demir k�l�� yap�m�nda kullan�l�r." gibi bir a��klama

    [TextArea(2, 4)]
    public string source;      // YEN�: "Da� Golemlerinden d��er." gibi bir kaynak bilgisi

    [Header("Item Ayarlar�")]
    public ItemType itemType; // YEN�: E�yan�n t�r� (Ekipman, Materyal vb.)
    public bool isStackable = false; // YEN�: Bu e�ya �st �ste birikebilir mi?
    public int maxStackSize = 1; // YEN�: Birikebilirse, en fazla ka� tane olabilir?

    public virtual void Use()
    {
        Debug.Log(name + " kullan�ld�.");
    }

    public void RemoveFromInventory()
    {
        Inventory.instance.Remove(this);
    }
}