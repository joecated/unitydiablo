using UnityEngine;
using System;
using System.Collections.Generic;

public class QuestLog : MonoBehaviour
{
    public static QuestLog instance;

    private Quest activeQuest;
    private int currentAmount = 0;
    private PlayerStats playerStats; // EXP vermek için PlayerStats referansý

    public List<Quest> completedQuests = new List<Quest>();
    public List<Quest> activeQuests = new List<Quest>();
    // UI'ýn dinleyeceði event. Parametreler: (Quest, mevcut miktar, gereken miktar)
    public event Action<Quest> OnQuestProgressUpdated;
    public event Action<Quest> OnQuestCompleted;

    void Awake()
    {
        // Singleton pattern: Sahnede sadece bir tane QuestLog olmasýný saðlar
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahne deðiþse bile görev listesi kaybolmasýn
        }
        else
        {
            Destroy(gameObject);
        }

        // Player objesinin üzerindeki PlayerStats script'ini bul ve referans al
        playerStats = GetComponent<PlayerStats>();
    }

    /// <summary>
    /// Yeni bir görevi aktif görev olarak ekler.
    /// </summary>
    public void AddQuest(Quest newQuest)
    {
        // --- YENÝ EKLENEN RESETLEME KISMI ---
        // Görevi kabul etmeden önce, içindeki tüm hedeflerin ilerlemesini sýfýrla.
        // Bu, oyunu her açtýðýnda veya görevi her aldýðýnda temiz bir baþlangýç yapýlmasýný saðlar.
        foreach (var objective in newQuest.objectives)
        {
            objective.isComplete = false; // "Tamamlandý" iþaretini kaldýr

            // Eðer hedef bir "KillObjective" ise, sayacýný da sýfýrla
            if (objective is KillObjective killObjective)
            {
                killObjective.currentAmount = 0;
            }
        }
        // ------------------------------------

        activeQuests.Add(newQuest);
        OnQuestProgressUpdated?.Invoke(newQuest);
    }

    /// <summary>
    /// Düþmanlar öldüðünde bu fonksiyon çaðrýlýr ve görev ilerlemesini kontrol eder.
    /// </summary>
    public void EnemyKilled(string enemyName)
    {
        // Tüm aktif görevleri kontrol et
        foreach (Quest quest in new List<Quest>(activeQuests))
        {
            // Görevin tüm hedeflerini kontrol et
            foreach (QuestObjective objective in quest.objectives)
            {
                // Eðer hedef bir "KillObjective" ise...
                if (objective is KillObjective killObjective)
                {
                    // Ve bu hedefin istediði düþman ile ölen düþman ayný ise...
                    if (!killObjective.isComplete && killObjective.targetPrefabName == enemyName)
                    {
                        killObjective.currentAmount++;
                        OnQuestProgressUpdated?.Invoke(quest); // UI'ý güncellemek için sinyal gönder
                        CheckQuestCompletion(quest); // Görev bitti mi diye kontrol et
                    }
                }
            }
        }
    }

    void Update()
    {
        foreach (Quest quest in new List<Quest>(activeQuests))
        {
            foreach (QuestObjective objective in quest.objectives)
            {
                if (objective is GoToObjective goToObjective)
                {
                    if (!goToObjective.isComplete && goToObjective.CheckProgress())
                    {
                        OnQuestProgressUpdated?.Invoke(quest);
                        CheckQuestCompletion(quest);
                    }
                }
            }
        }
    }

    void CheckQuestCompletion(Quest quest)
    {
        foreach (QuestObjective objective in quest.objectives)
        {
            // Eðer hedeflerden BÝRÝ BÝLE tamamlanmamýþsa, fonksiyondan çýk
            if (!objective.CheckProgress())
            {
                return;
            }
        }

        // Eðer buraya kadar gelebildiysek, tüm hedefler tamamlanmýþ demektir.
        Debug.Log(quest.questName + " görevi tamamlandý!");

        if (playerStats != null)
        {
            playerStats.GainXp(quest.experienceReward);
            Debug.Log(quest.experienceReward + " EXP kazanýldý!");
            playerStats.AddGold(quest.goldReward); // YENÝ: Altýn ödülünü veriyoruz
        }
        
        // --- GÖREV LÝSTELERÝNÝ GÜNCELLE ---
        completedQuests.Add(quest); // YENÝ: Görevi tamamlananlar listesine ekle
        activeQuests.Remove(quest);

        // --- YENÝ SÝNYALÝ GÖNDER ---
        // Ödülleri verdikten sonra UI'a haber veriyoruz.
        OnQuestCompleted?.Invoke(quest);
        // Artýk bu silme iþlemi hataya neden olmayacak çünkü döngü kopyada çalýþýyor.
        activeQuests.Remove(quest);
        OnQuestProgressUpdated?.Invoke(null); // UI'ý temizle
    }
}