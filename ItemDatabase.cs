// ItemDatabase.cs (YEN� SCR�PT)
// �simlerine g�re e�yalar� bulmam�z� sa�layan ScriptableObject.
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory/Item Database")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> allItems;

    public Item GetItemByName(string name)
    {
        return allItems.Find(item => item.name == name);
    }
}