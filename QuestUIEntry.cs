using UnityEngine;
using TMPro;
using System.Text;

public class QuestUIEntry : MonoBehaviour
{
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI questObjectiveText;

    public void Setup(Quest quest)
    {
        questNameText.text = quest.questName;

        StringBuilder sb = new StringBuilder();
        foreach (var objective in quest.objectives)
        {
            string status = "";

            if (objective is KillObjective killObjective)
            {
                status = killObjective.isComplete ? "<color=green>(Tamamland�)</color>" : $"({killObjective.currentAmount} / {killObjective.requiredAmount})";
                sb.AppendLine($"- {killObjective.targetPrefabName} �ld�r {status}");
            }
            else if (objective is GoToObjective goToObjective)
            {
                status = goToObjective.isComplete ? "<color=green>(Tamamland�)</color>" : "(Bekliyor)";
                // Art�k do�ru de�i�keni kullan�yoruz
                sb.AppendLine($"- {goToObjective.displayName}'a git {status}");
            }
        }
        questObjectiveText.text = sb.ToString();
    }
}