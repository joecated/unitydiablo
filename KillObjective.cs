// KillObjective.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New Kill Objective", menuName = "Quests/Objectives/Kill Objective")]
public class KillObjective : QuestObjective
{
    public string targetPrefabName;
    public int requiredAmount;
    [HideInInspector] public int currentAmount; // �lerlemeyi takip etmek i�in

    public override bool CheckProgress()
    {
        isComplete = (currentAmount >= requiredAmount);
        return isComplete;
    }
}