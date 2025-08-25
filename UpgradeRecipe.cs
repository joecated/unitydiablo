// UpgradeRecipe.cs (YENÝ SCRÝPT)
using System.Collections.Generic;
using UnityEngine;

// Bu küçük class, tarif içinde "5 demir, 2 odun" gibi listeler yapmamýzý saðlar
[System.Serializable]
public class MaterialCost
{
    public MaterialItem material;
    public int quantity;
}

[CreateAssetMenu(fileName = "New Upgrade Recipe", menuName = "Crafting/Upgrade Recipe")]
public class UpgradeRecipe : ScriptableObject
{
    [Header("Giriþ")]
    public Equipment itemToUpgrade; // Geliþtirilecek ana eþya (örn: Demir Kýlýç)
    public List<MaterialCost> materialsRequired; // Gereken materyaller
    public int goldCost; // Gereken altýn

    [Header("Çýkýþ")]
    public Equipment upgradedItemResult; // Geliþtirme sonrasý ortaya çýkacak eþya (örn: Demir Kýlýç +1)
}