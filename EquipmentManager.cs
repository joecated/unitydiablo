// EquipmentManager.cs (TAM SÜRÜM)
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    #region Singleton
    public static EquipmentManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of EquipmentManager found!");
            return;
        }
        instance = this;
    }
    #endregion

    public Equipment[] currentEquipment;
    private Inventory inventory;
    private PlayerStats playerStats;

    // UI'ý güncellemek için event
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem);
    public event OnEquipmentChanged onEquipmentChanged;

    void Start()
    {
        inventory = Inventory.instance;
        playerStats = FindObjectOfType<PlayerStats>();

        int numSlots = System.Enum.GetNames(typeof(EquipSlot)).Length;
        currentEquipment = new Equipment[numSlots];
    }

    public void Equip(Equipment newItem)
    {
        int slotIndex = (int)newItem.equipSlot;
        Equipment oldItem = null;

        // Eðer o slotta zaten bir eþya varsa...
        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem); // Eski eþyayý envantere geri ekle
            Unequip(slotIndex); // Eski eþyanýn stat'larýný kaldýr
        }

        // Yeni eþyayý kuþan
        currentEquipment[slotIndex] = newItem;

        // Yeni eþyanýn stat'larýný oyuncuya ekle
        playerStats.maxHealth.AddModifier(newItem.healthModifier); // YENÝ
        playerStats.maxMana.AddModifier(newItem.manaModifier);     // YENÝ
        playerStats.armor.AddModifier(newItem.armorModifier);
        playerStats.magicResist.AddModifier(newItem.magicResistModifier);
        playerStats.damage.AddModifier(newItem.damageModifier);
        playerStats.abilityPower.AddModifier(newItem.abilityPowerModifier);
        playerStats.critChance.AddModifier(newItem.critChanceModifier);
        playerStats.critDamage.AddModifier(newItem.critDamageModifier);
        playerStats.cooldownReduction.AddModifier(newItem.cooldownReductionModifier);

        // Event'i tetikle
        if (onEquipmentChanged != null)
        {
            onEquipmentChanged.Invoke(newItem, oldItem);
        }
    }

    public void Unequip(int slotIndex)
    {
        Equipment oldItem = currentEquipment[slotIndex];
        if (oldItem != null)
        {
            // Önce eþyanýn envanterde yeri olup olmadýðýný kontrol et
            // ve envantere ekle.
            bool wasAdded = inventory.Add(oldItem);

            if (wasAdded)
            {
                // Eski eþyanýn stat'larýný oyuncudan kaldýr
                
                playerStats.maxHealth.RemoveModifier(oldItem.healthModifier); // YENÝ
                playerStats.maxMana.RemoveModifier(oldItem.manaModifier);     // YENÝ
                playerStats.armor.RemoveModifier(oldItem.armorModifier);
                playerStats.magicResist.RemoveModifier(oldItem.magicResistModifier);
                playerStats.damage.RemoveModifier(oldItem.damageModifier);
                playerStats.abilityPower.RemoveModifier(oldItem.abilityPowerModifier);
                playerStats.critChance.RemoveModifier(oldItem.critChanceModifier);
                playerStats.critDamage.RemoveModifier(oldItem.critDamageModifier);
                playerStats.cooldownReduction.RemoveModifier(oldItem.cooldownReductionModifier);

                currentEquipment[slotIndex] = null;

                // UI'ý güncellemek için event'i tetikle
                onEquipmentChanged?.Invoke(null, oldItem);
            }
        }
    }

    // (Ýsteðe baðlý) Tüm eþyalarý çýkarmak için
    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
    }
}