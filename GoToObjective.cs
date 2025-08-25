// GoToObjective.cs
using UnityEngine;

[CreateAssetMenu(fileName = "New GoTo Objective", menuName = "Quests/Objectives/GoTo Objective")]
public class GoToObjective : QuestObjective
{

    // --- BU SATIRI EKLE ---
    public string displayName; // "De�irmen", "Orman ��k���" gibi UI'da g�r�necek isim.
    // ----------------------

    public Vector3 targetPosition;
    public float acceptanceRadius = 3f; // Ne kadar yakla��nca tamamlanm�� say�laca��

    public override bool CheckProgress()
    {
        // Oyuncunun pozisyonunu bulup hedefe olan mesafeyi kontrol et
        Transform player = FindObjectOfType<PlayerController>().transform;
        if (player != null && Vector3.Distance(player.position, targetPosition) <= acceptanceRadius)
        {
            isComplete = true;
        }
        return isComplete;
    }
}