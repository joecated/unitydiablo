// ShopData.cs (YEN� SCR�PT)
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shop", menuName = "Shop/Shop Data")]
public class ShopData : ScriptableObject
{
    public List<Item> itemsForSale;
}