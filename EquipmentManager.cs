// EquipmentManager.cs (TAM S�R�M)
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

    // UI'� g�ncellemek i�in event
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

        // E�er o slotta zaten bir e�ya varsa...
        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];
            inventory.Add(oldItem); // Eski e�yay� envantere geri ekle
            Unequip(slotIndex); // Eski e�yan�n stat'lar�n� kald�r
        }

        // Yeni e�yay� ku�an
        currentEquipment[slotIndex] = newItem;

        // Yeni e�yan�n stat'lar�n� oyuncuya ekle
        playerStats.maxHealth.AddModifier(newItem.healthModifier); // YEN�
        playerStats.maxMana.AddModifier(newItem.manaModifier);     // YEN�
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
            // �nce e�yan�n envanterde yeri olup olmad���n� kontrol et
            // ve envantere ekle.
            bool wasAdded = inventory.Add(oldItem);

            if (wasAdded)
            {
                // Eski e�yan�n stat'lar�n� oyuncudan kald�r
                
                playerStats.maxHealth.RemoveModifier(oldItem.healthModifier); // YEN�
                playerStats.maxMana.RemoveModifier(oldItem.manaModifier);     // YEN�
                playerStats.armor.RemoveModifier(oldItem.armorModifier);
                playerStats.magicResist.RemoveModifier(oldItem.magicResistModifier);
                playerStats.damage.RemoveModifier(oldItem.damageModifier);
                playerStats.abilityPower.RemoveModifier(oldItem.abilityPowerModifier);
                playerStats.critChance.RemoveModifier(oldItem.critChanceModifier);
                playerStats.critDamage.RemoveModifier(oldItem.critDamageModifier);
                playerStats.cooldownReduction.RemoveModifier(oldItem.cooldownReductionModifier);

                currentEquipment[slotIndex] = null;

                // UI'� g�ncellemek i�in event'i tetikle
                onEquipmentChanged?.Invoke(null, oldItem);
            }
        }
    }

    // (�ste�e ba�l�) T�m e�yalar� ��karmak i�in
    public void UnequipAll()
    {
        for (int i = 0; i < currentEquipment.Length; i++)
        {
            Unequip(i);
        }
    }
}