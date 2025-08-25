// Quest.cs
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    [Header("Görev Bilgileri")]
    public string questName;
    [TextArea(3, 10)]
    public string description;

    public event Action<Quest> OnQuestProgressUpdated;

    

    [Header("Görev Hedefleri")]
    public QuestObjective[] objectives; // Artýk bir hedef listesi tutuyoruz

    [Header("Ödüller")] // YENÝ
    public int experienceReward; // YENÝ
    public int goldReward; // HATA BURADA: Bu satýr muhtemelen eksik veya yanlýþ yazýlmýþ.
}