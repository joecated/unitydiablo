// EnemyStats.cs (GEL��M�� LOOT S�STEM�)
using UnityEngine;

// Bu helper class, Inspector'da her bir loot i�in �ans belirlememizi sa�lar.
[System.Serializable]
public class LootDrop
{
    public Item item;
    [Range(0f, 1f)] // 0 = %0 �ans, 1 = %100 �ans
    public float dropChance;
}

public class EnemyStats : CharacterStats
{
    public int xpValue = 15;
    public GameObject itemDropPrefab;

    [Header("Ganimet Tablosu")]
    public LootDrop[] lootTable; // ESK� possibleLoot D�Z�S�N�N YER�NE BU GELD�

    public override void Die()
    {
        base.Die();

        // Fizik ve Di�er Sistemler (Ayn� kal�yor)
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

        // YEN� LOOT S�STEM�N� �A�IR
        DropLoot();

        Destroy(gameObject);
    }

    // DropLoot fonksiyonu art�k �ok daha ak�ll�
    void DropLoot()
    {
        if (itemDropPrefab == null || lootTable.Length == 0) return;

        // B�t�n loot tablosunu gez
        foreach (LootDrop drop in lootTable)
        {
            // Her bir e�ya i�in zar� at (0.0 ile 1.0 aras�nda)
            float randomValue = Random.Range(0f, 1f);

            // E�er zardaki say�, e�yan�n d��me �ans�ndan k���k veya e�itse, E�YA D��T�!
            if (randomValue <= drop.dropChance)
            {
                // Yere d��en e�ya objesini olu�tur
                GameObject droppedItemObject = Instantiate(itemDropPrefab, transform.position, Quaternion.identity);
                droppedItemObject.GetComponent<ItemPickup>().item = drop.item;
                Debug.Log(drop.item.name + " d��t�!");
            }
        }
    }
}