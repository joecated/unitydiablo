// UpgradeRecipe.cs (YEN� SCR�PT)
using System.Collections.Generic;
using UnityEngine;

// Bu k���k class, tarif i�inde "5 demir, 2 odun" gibi listeler yapmam�z� sa�lar
[System.Serializable]
public class MaterialCost
{
    public MaterialItem material;
    public int quantity;
}

[CreateAssetMenu(fileName = "New Upgrade Recipe", menuName = "Crafting/Upgrade Recipe")]
public class UpgradeRecipe : ScriptableObject
{
    [Header("Giri�")]
    public Equipment itemToUpgrade; // Geli�tirilecek ana e�ya (�rn: Demir K�l��)
    public List<MaterialCost> materialsRequired; // Gereken materyaller
    public int goldCost; // Gereken alt�n

    [Header("��k��")]
    public Equipment upgradedItemResult; // Geli�tirme sonras� ortaya ��kacak e�ya (�rn: Demir K�l�� +1)
}