// PlayerData.cs (DO�RU VE BAS�T HAL�)

[System.Serializable]
public class PlayerData
{
    // PlayerStats
    public int level;
    public int currentXp;
    public int xpToNextLevel;
    public float maxHealth;
    public int currentHealth;
    public float maxMana;
    public int currentMana;
    public float damage;
    public float armor;
    public float magicResist;

    // Inventory
    public string[] inventoryItemNames;

    // Equipment
    public string[] equippedItemNames;

    // CONSTRUCTOR (YAPICI METOT) S�L�ND�!
    // Art�k GameManager veya ba�ka bir �eyi tan�m�yor. Sadece bir veri konteyneri.
}