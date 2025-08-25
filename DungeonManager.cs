// DungeonManager.cs
using UnityEngine;
using System.Collections; // Coroutine i�in eklendi

public class DungeonManager : MonoBehaviour
{
    [Header("G�rev Ayarlar�")]
    [Tooltip("Boss'un ��kmas� i�in kesilmesi gereken yarat�k say�s�.")]
    public int killsToSpawnBoss = 3;

    [Header("Objektif Referanslar�")]
    [Tooltip("Boss olarak spawn olacak prefab.")]
    public GameObject bossPrefab;
    [Tooltip("Boss'un spawn olaca�� pozisyonu belirler.")]
    public Transform bossSpawnPoint;
    [Tooltip("Boss �l�nce aktif olacak ��k�� portal�.")]
    public GameObject returnPortal;

    private int enemiesKilled = 0;
    private bool bossSpawned = false;

    void Start()
    {
        if (returnPortal != null)
            returnPortal.SetActive(false);
        else
            Debug.LogError("HATA: ��k�� portal� DungeonManager'a atanmam��!", this.gameObject);
    }

    public void OnEnemyKilled()
    {
        if (bossSpawned) return;

        enemiesKilled++;
        Debug.Log("Normal yarat�k �ld�! Gereken i�in " + enemiesKilled + "/" + killsToSpawnBoss);

        if (enemiesKilled >= killsToSpawnBoss)
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab == null || bossSpawnPoint == null)
        {
            Debug.LogError("HATA: Boss Prefab veya Spawn Point atanmam��!", this.gameObject);
            return;
        }

        Debug.Log("BOSS SPAWN OLUYOR!");
        GameObject bossInstance = Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
        bossSpawned = true;

        CharacterStats bossStats = bossInstance.GetComponent<CharacterStats>();
        if (bossStats != null)
        {
            Debug.Log("Boss'un statlar� g��lendiriliyor.");
            bossStats.maxHealth.AddModifier(500);
            
            bossStats.armor.AddModifier(50);
            bossStats.magicResist.AddModifier(50);
        }
    }

    public void OnBossKilled()
    {
        Debug.Log("BOSS MA�LUP ED�LD�! Zindan tamamland�.");
        if (returnPortal != null)
        {
            Debug.Log("��k�� portal� aktif ediliyor.");
            returnPortal.SetActive(true);
        }

        // G�rev sistemine haber ver.
        // QuestManager.Instance.CompleteQuest("Zindan�Temizle");
    }
}