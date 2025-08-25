// GameManager.cs (NÝHAÝ, SAÐLAM VE DÝNAMÝK HALÝ)
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- Singleton Deseni: Diðer script'ler bu script'e "GameManager.Instance" yazarak ulaþabilecek ---
    public static GameManager Instance { get; private set; }

    // --- Referanslar: Artýk public deðiþkenler deðil, public "özellikler" (properties) ---
    // Bu sayede diðer script'ler bu referanslarý okuyabilir ama sadece GameManager deðiþtirebilir.
    public PlayerStats playerStats { get; private set; }
    public Inventory inventory { get; private set; }
    public EquipmentManager equipmentManager { get; private set; }

    // ItemDatabase public kalabilir çünkü o oyuncuya deðil, projenin kendisine aittir.
    public ItemDatabase itemDatabase;

    private bool referencesFound = false; // Referanslarýn bulunduðunu kontrol eden bayrak

    void Awake()
    {
        // --- YENÝ: Singleton Kurulumu ---
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // ESKÝ KODU SÝLÝYORUZ! Buradaki aceleci FindFirstObjectByType'larý kaldýrýyoruz.
        // playerStats = FindFirstObjectByType<PlayerStats>();
        // inventory = FindFirstObjectByType<Inventory>();
        // equipmentManager = FindFirstObjectByType<EquipmentManager>();
    }

    void Update()
    {
        // --- YENÝ: Sabýrlý Referans Bulma Mantýðý ---
        // Eðer referanslarý henüz bulamadýysak...
        if (!referencesFound)
        {
            // Kalýcý oyuncunun var olup olmadýðýný kontrol et
            if (PlayerPersistence.instance != null)
            {
                // Oyuncu objesini al
                GameObject playerObject = PlayerPersistence.instance.gameObject;

                // Oyuncudan ihtiyacýmýz olan bileþenleri al ve referanslara ata
                playerStats = playerObject.GetComponent<PlayerStats>();
                inventory = playerObject.GetComponent<Inventory>();
                equipmentManager = playerObject.GetComponent<EquipmentManager>();

                // Eðer tüm referanslarý baþarýyla bulduysak...
                if (playerStats != null && inventory != null && equipmentManager != null)
                {
                    Debug.Log("<color=lime>GameManager: Tüm oyuncu referanslarý baþarýyla yakalandý!</color>");
                    referencesFound = true; // Bayraðý kaldýr ki bu bloða bir daha girmeyelim.
                }
            }
        }
    }

    // --- BU FONKSÝYONLARA HÝÇ DOKUNULMADI ---
    public void SaveGame()
    {
        if (!referencesFound)
        {
            Debug.LogWarning("Oyuncu referanslarý bulunmadan oyun kaydedilemez!");
            return;
        }

        // 1. Boþ bir veri kutusu (PlayerData) oluþtur.
        PlayerData data = new PlayerData();

        // 2. Kutuyu oyuncunun güncel bilgileriyle doldur.
        data.level = playerStats.level;
        data.currentXp = playerStats.currentXp;
        data.xpToNextLevel = playerStats.xpToNextLevel;
        data.maxHealth = playerStats.maxHealth.GetValue();
        data.currentHealth = playerStats.currentHealth;
        data.maxMana = playerStats.maxMana.GetValue();
        data.currentMana = playerStats.currentMana;
        data.damage = playerStats.damage.GetValue();
        data.armor = playerStats.armor.GetValue();
        data.magicResist = playerStats.magicResist.GetValue();

        // 3. Envanteri ve ekipmaný doldur.
        data.inventoryItemNames = new string[inventory.items.Count];
        for (int i = 0; i < inventory.items.Count; i++)
        {
            data.inventoryItemNames[i] = inventory.items[i].name;
        }

        data.equippedItemNames = new string[equipmentManager.currentEquipment.Length];
        for (int i = 0; i < equipmentManager.currentEquipment.Length; i++)
        {
            if (equipmentManager.currentEquipment[i] != null)
            {
                data.equippedItemNames[i] = equipmentManager.currentEquipment[i].name;
            }
        }

        // 4. Dolu kutuyu, artýk PlayerData bekleyen SaveSystem'e gönder.
        SaveSystem.SavePlayer(data);
    }

    // --- BU FONKSÝYONLARA HÝÇ DOKUNULMADI ---
    //public void LoadGame()
    //{
    //    if (!referencesFound)
    //    {
    //        Debug.LogWarning("Oyuncu referanslarý bulunmadan oyun yüklenemez!");
    //        return;
    //    }

    //    PlayerData data = SaveSystem.LoadPlayer();
    //    if (data == null) return;

    //    playerStats.LoadStats(data);

    //    inventory.items.Clear();
    //    equipmentManager.UnequipAll();

    //    foreach (string itemName in data.inventoryItemNames)
    //    {
    //        inventory.Add(itemDatabase.GetItemByName(itemName));
    //    }

    //    for (int i = 0; i < data.equippedItemNames.Length; i++)
    //    {
    //        if (!string.IsNullOrEmpty(data.equippedItemNames[i]))
    //        {
    //            Item itemToEquip = itemDatabase.GetItemByName(data.equippedItemNames[i]);
    //            if (itemToEquip != null)
    //            {
    //                equipmentManager.Equip(itemToEquip as Equipment);
    //            }
    //        }
    //    }

    //    foreach (Equipment equippedItem in equipmentManager.currentEquipment)
    //    {
    //        if (equippedItem != null)
    //        {
    //            inventory.Remove(equippedItem);
    //        }
    //    }

    //    Debug.Log("Veriler baþarýyla yüklendi ve uygulandý.");
    //}
}