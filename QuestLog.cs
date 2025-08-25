using UnityEngine;
using System;
using System.Collections.Generic;

public class QuestLog : MonoBehaviour
{
    public static QuestLog instance;

    private Quest activeQuest;
    private int currentAmount = 0;
    private PlayerStats playerStats; // EXP vermek i�in PlayerStats referans�

    public List<Quest> completedQuests = new List<Quest>();
    public List<Quest> activeQuests = new List<Quest>();
    // UI'�n dinleyece�i event. Parametreler: (Quest, mevcut miktar, gereken miktar)
    public event Action<Quest> OnQuestProgressUpdated;
    public event Action<Quest> OnQuestCompleted;

    void Awake()
    {
        // Singleton pattern: Sahnede sadece bir tane QuestLog olmas�n� sa�lar
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Sahne de�i�se bile g�rev listesi kaybolmas�n
        }
        else
        {
            Destroy(gameObject);
        }

        // Player objesinin �zerindeki PlayerStats script'ini bul ve referans al
        playerStats = GetComponent<PlayerStats>();
    }

    /// <summary>
    /// Yeni bir g�revi aktif g�rev olarak ekler.
    /// </summary>
    public void AddQuest(Quest newQuest)
    {
        // --- YEN� EKLENEN RESETLEME KISMI ---
        // G�revi kabul etmeden �nce, i�indeki t�m hedeflerin ilerlemesini s�f�rla.
        // Bu, oyunu her a�t���nda veya g�revi her ald���nda temiz bir ba�lang�� yap�lmas�n� sa�lar.
        foreach (var objective in newQuest.objectives)
        {
            objective.isComplete = false; // "Tamamland�" i�aretini kald�r

            // E�er hedef bir "KillObjective" ise, sayac�n� da s�f�rla
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
    /// D��manlar �ld���nde bu fonksiyon �a�r�l�r ve g�rev ilerlemesini kontrol eder.
    /// </summary>
    public void EnemyKilled(string enemyName)
    {
        // T�m aktif g�revleri kontrol et
        foreach (Quest quest in new List<Quest>(activeQuests))
        {
            // G�revin t�m hedeflerini kontrol et
            foreach (QuestObjective objective in quest.objectives)
            {
                // E�er hedef bir "KillObjective" ise...
                if (objective is KillObjective killObjective)
                {
                    // Ve bu hedefin istedi�i d��man ile �len d��man ayn� ise...
                    if (!killObjective.isComplete && killObjective.targetPrefabName == enemyName)
                    {
                        killObjective.currentAmount++;
                        OnQuestProgressUpdated?.Invoke(quest); // UI'� g�ncellemek i�in sinyal g�nder
                        CheckQuestCompletion(quest); // G�rev bitti mi diye kontrol et
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
            // E�er hedeflerden B�R� B�LE tamamlanmam��sa, fonksiyondan ��k
            if (!objective.CheckProgress())
            {
                return;
            }
        }

        // E�er buraya kadar gelebildiysek, t�m hedefler tamamlanm�� demektir.
        Debug.Log(quest.questName + " g�revi tamamland�!");

        if (playerStats != null)
        {
            playerStats.GainXp(quest.experienceReward);
            Debug.Log(quest.experienceReward + " EXP kazan�ld�!");
            playerStats.AddGold(quest.goldReward); // YEN�: Alt�n �d�l�n� veriyoruz
        }
        
        // --- G�REV L�STELER�N� G�NCELLE ---
        completedQuests.Add(quest); // YEN�: G�revi tamamlananlar listesine ekle
        activeQuests.Remove(quest);

        // --- YEN� S�NYAL� G�NDER ---
        // �d�lleri verdikten sonra UI'a haber veriyoruz.
        OnQuestCompleted?.Invoke(quest);
        // Art�k bu silme i�lemi hataya neden olmayacak ��nk� d�ng� kopyada �al���yor.
        activeQuests.Remove(quest);
        OnQuestProgressUpdated?.Invoke(null); // UI'� temizle
    }
}