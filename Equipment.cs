// Equipment.cs (YEN� SCR�PT)
// Item'dan miras al�r ve ku�an�labilir e�yalar�n �zelliklerini tutar.
using UnityEngine;

// Ekipman tiplerini belirlemek i�in bir enum
public enum EquipmentSlot { Head, Chest, Legs, Weapon, Shield, Feet }

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot; // Bu e�ya hangi slota tak�l�yor?

    public int armorModifier;
    public int magicResistModifier;
    public int damageModifier;
    public int abilityPowerModifier;
    public float critChanceModifier;
    public float critDamageModifier;
    public float cooldownReductionModifier;
    public int healthModifier;
    public int manaModifier;
    [Header("�zel Efektler")]
    public bool grantsBarrierEffect = false; // YEN�: Bu e�ya bariyer verir mi?
    public float barrierEffectCooldown = 45f;



    // E�yay� envanterden kullanma (ku�anma) mant���
    public override void Use()
    {
        base.Use();
        // Ekipman y�neticisine bu e�yay� ku�anmas�n� s�yle
        EquipmentManager.instance.Equip(this);
        // E�yay� envanterden kald�r
        RemoveFromInventory();
    }
}