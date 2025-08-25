// Equipment.cs (YENÝ SCRÝPT)
// Item'dan miras alýr ve kuþanýlabilir eþyalarýn özelliklerini tutar.
using UnityEngine;

// Ekipman tiplerini belirlemek için bir enum
public enum EquipmentSlot { Head, Chest, Legs, Weapon, Shield, Feet }

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot; // Bu eþya hangi slota takýlýyor?

    public int armorModifier;
    public int magicResistModifier;
    public int damageModifier;
    public int abilityPowerModifier;
    public float critChanceModifier;
    public float critDamageModifier;
    public float cooldownReductionModifier;
    public int healthModifier;
    public int manaModifier;
    [Header("Özel Efektler")]
    public bool grantsBarrierEffect = false; // YENÝ: Bu eþya bariyer verir mi?
    public float barrierEffectCooldown = 45f;



    // Eþyayý envanterden kullanma (kuþanma) mantýðý
    public override void Use()
    {
        base.Use();
        // Ekipman yöneticisine bu eþyayý kuþanmasýný söyle
        EquipmentManager.instance.Equip(this);
        // Eþyayý envanterden kaldýr
        RemoveFromInventory();
    }
}