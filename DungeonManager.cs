// DungeonManager.cs
using UnityEngine;
using System.Collections; // Coroutine için eklendi

public class DungeonManager : MonoBehaviour
{
    [Header("Görev Ayarlarý")]
    [Tooltip("Boss'un çýkmasý için kesilmesi gereken yaratýk sayýsý.")]
    public int killsToSpawnBoss = 3;

    [Header("Objektif Referanslarý")]
    [Tooltip("Boss olarak spawn olacak prefab.")]
    public GameObject bossPrefab;
    [Tooltip("Boss'un spawn olacaðý pozisyonu belirler.")]
    public Transform bossSpawnPoint;
    [Tooltip("Boss ölünce aktif olacak çýkýþ portalý.")]
    public GameObject returnPortal;

    private int enemiesKilled = 0;
    private bool bossSpawned = false;

    void Start()
    {
        if (returnPortal != null)
            returnPortal.SetActive(false);
        else
            Debug.LogError("HATA: Çýkýþ portalý DungeonManager'a atanmamýþ!", this.gameObject);
    }

    public void OnEnemyKilled()
    {
        if (bossSpawned) return;

        enemiesKilled++;
        Debug.Log("Normal yaratýk öldü! Gereken için " + enemiesKilled + "/" + killsToSpawnBoss);

        if (enemiesKilled >= killsToSpawnBoss)
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab == null || bossSpawnPoint == null)
        {
            Debug.LogError("HATA: Boss Prefab veya Spawn Point atanmamýþ!", this.gameObject);
            return;
        }

        Debug.Log("BOSS SPAWN OLUYOR!");
        GameObject bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
        bossSpawned = true;

        CharacterStats bossStats = bossInstance.GetComponent<CharacterStats>();
        if (bossStats != null)
        {
            Debug.Log("Boss'un statlarý güçlendiriliyor.");
            bossStats.maxHealth.AddModifier(500);
            
            bossStats.armor.AddModifier(50);
            bossStats.magicResist.AddModifier(50);
        }
    }

    public void OnBossKilled()
    {
        Debug.Log("BOSS MAÐLUP EDÝLDÝ! Zindan tamamlandý.");
        if (returnPortal != null)
        {
            Debug.Log("Çýkýþ portalý aktif ediliyor.");
            returnPortal.SetActive(true);
        }

        // Görev sistemine haber ver.
        // QuestManager.Instance.CompleteQuest("ZindanýTemizle");
    }
}