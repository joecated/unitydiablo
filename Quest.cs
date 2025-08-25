// Quest.cs
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    [Header("G�rev Bilgileri")]
    public string questName;
    [TextArea(3, 10)]
    public string description;

    public event Action<Quest> OnQuestProgressUpdated;

    

    [Header("G�rev Hedefleri")]
    public QuestObjective[] objectives; // Art�k bir hedef listesi tutuyoruz

    [Header("�d�ller")] // YEN�
    public int experienceReward; // YEN�
    public int goldReward; // HATA BURADA: Bu sat�r muhtemelen eksik veya yanl�� yaz�lm��.
}