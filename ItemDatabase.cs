// ItemDatabase.cs (YENÝ SCRÝPT)
// Ýsimlerine göre eþyalarý bulmamýzý saðlayan ScriptableObject.
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