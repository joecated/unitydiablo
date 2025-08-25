// EnemyStats.cs (GELÝÞMÝÞ LOOT SÝSTEMÝ)
using UnityEngine;

// Bu helper class, Inspector'da her bir loot için þans belirlememizi saðlar.
[System.Serializable]
public class LootDrop
{
    public Item item;
    [Range(0f, 1f)] // 0 = %0 þans, 1 = %100 þans
    public float dropChance;
}

public class EnemyStats : CharacterStats
{
    public int xpValue = 15;
    public GameObject itemDropPrefab;

    [Header("Ganimet Tablosu")]
    public LootDrop[] lootTable; // ESKÝ possibleLoot DÝZÝSÝNÝN YERÝNE BU GELDÝ

    public override void Die()
    {
        base.Die();

        // Fizik ve Diðer Sistemler (Ayný kalýyor)
        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.enabled = false;
        if (QuestLog.instance != null)
        {
            QuestLog.instance.EnemyKilled(gameObject.name.Replace("(Clone)", "").Trim());
        }
        PlayerStats playerStats = FindFirstObjectByType<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.GainXp(xpValue);
        }

        // YENÝ LOOT SÝSTEMÝNÝ ÇAÐIR
        DropLoot();

        Destroy(gameObject);
    }

    // DropLoot fonksiyonu artýk çok daha akýllý
    void DropLoot()
    {
        if (itemDropPrefab == null || lootTable.Length == 0) return;

        // Bütün loot tablosunu gez
        foreach (LootDrop drop in lootTable)
        {
            // Her bir eþya için zarý at (0.0 ile 1.0 arasýnda)
            float randomValue = Random.Range(0f, 1f);

            // Eðer zardaki sayý, eþyanýn düþme þansýndan küçük veya eþitse, EÞYA DÜÞTÜ!
            if (randomValue <= drop.dropChance)
            {
                // Yere düþen eþya objesini oluþtur
                GameObject droppedItemObject = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
                droppedItemObject.GetComponent<ItemPickup>().item = drop.item;
                Debug.Log(drop.item.name + " düþtü!");
            }
        }
    }
}