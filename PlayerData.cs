// PlayerData.cs (DOÐRU VE BASÝT HALÝ)

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

    // CONSTRUCTOR (YAPICI METOT) SÝLÝNDÝ!
    // Artýk GameManager veya baþka bir þeyi tanýmýyor. Sadece bir veri konteyneri.
}