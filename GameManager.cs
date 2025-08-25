// GameManager.cs (N�HA�, SA�LAM VE D�NAM�K HAL�)
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- Singleton Deseni: Di�er script'ler bu script'e "GameManager.Instance" yazarak ula�abilecek ---
    public static GameManager Instance { get; private set; }

    // --- Referanslar: Art�k public de�i�kenler de�il, public "�zellikler" (properties) ---
    // Bu sayede di�er script'ler bu referanslar� okuyabilir ama sadece GameManager de�i�tirebilir.
    public PlayerStats playerStats { get; private set; }
    public Inventory inventory { get; private set; }
    public EquipmentManager equipmentManager { get; private set; }

    // ItemDatabase public kalabilir ��nk� o oyuncuya de�il, projenin kendisine aittir.
    public ItemDatabase itemDatabase;

    private bool referencesFound = false; // Referanslar�n bulundu�unu kontrol eden bayrak

    void Awake()
    {
        // --- YEN�: Singleton Kurulumu ---
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        // ESK� KODU S�L�YORUZ! Buradaki aceleci FindFirstObjectByType'lar� kald�r�yoruz.
        // playerStats = FindFirstObjectByType<PlayerStats>();
        // inventory = FindFirstObjectByType<Inventory>();
        // equipmentManager = FindFirstObjectByType<EquipmentManager>();
    }

    void Update()
    {
        // --- YEN�: Sab�rl� Referans Bulma Mant��� ---
        // E�er referanslar� hen�z bulamad�ysak...
        if (!referencesFound)
        {
            // Kal�c� oyuncunun var olup olmad���n� kontrol et
            if (PlayerPersistence.instance != null)
            {
                // Oyuncu objesini al
                GameObject playerObject = PlayerPersistence.instance.gameObject;

                // Oyuncudan ihtiyac�m�z olan bile�enleri al ve referanslara ata
                playerStats = playerObject.GetComponent<PlayerStats>();
                inventory = playerObject.GetComponent<Inventory>();
                equipmentManager = playerObject.GetComponent<EquipmentManager>();

                // E�er t�m referanslar� ba�ar�yla bulduysak...
                if (playerStats != null && inventory != null && equipmentManager != null)
                {
                    Debug.Log("<color=lime>GameManager: T�m oyuncu referanslar� ba�ar�yla yakaland�!</color>");
                    referencesFound = true; // Bayra�� kald�r ki bu blo�a bir daha girmeyelim.
                }
            }
        }
    }

    // --- BU FONKS�YONLARA H�� DOKUNULMADI ---
    public void SaveGame()
    {
        if (!referencesFound)
        {
            Debug.LogWarning("Oyuncu referanslar� bulunmadan oyun kaydedilemez!");
            return;
        }

        // 1. Bo� bir veri kutusu (PlayerData) olu�tur.
        PlayerData data = new PlayerData();

        // 2. Kutuyu oyuncunun g�ncel bilgileriyle doldur.
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

        // 3. Envanteri ve ekipman� doldur.
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

        // 4. Dolu kutuyu, art�k PlayerData bekleyen SaveSystem'e g�nder.
        SaveSystem.SavePlayer(data);
    }

    // --- BU FONKS�YONLARA H�� DOKUNULMADI ---
    //public void LoadGame()
    //{
    //    if (!referencesFound)
    //    {
    //        Debug.LogWarning("Oyuncu referanslar� bulunmadan oyun y�klenemez!");
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

    //    Debug.Log("Veriler ba�ar�yla y�klendi ve uyguland�.");
    //}
}