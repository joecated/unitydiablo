// QuestUI.cs (HATALARI ÇÖZEN GÜNCEL HALÝ)
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public GameObject questPanel;
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI objectiveText;

    void Start()
    {
        questPanel.SetActive(false);
        if (QuestLog.instance != null)
        {
            // Abone olurken kullandýðýmýz fonksiyonun imzasý (UpdateUI) artýk uyumlu.
            QuestLog.instance.OnQuestProgressUpdated += UpdateUI;
        }
    }

    private void OnDestroy()
    {
        if (QuestLog.instance != null)
        {
            QuestLog.instance.OnQuestProgressUpdated -= UpdateUI;
        }
    }

    // GÜNCELLEME: Fonksiyon artýk 3 parametre yerine sadece 1 tane "Quest" parametresi alýyor.
    private void UpdateUI(Quest quest)
    {
        if (quest != null && questPanel != null)
        {
            questPanel.SetActive(true);
            questNameText.text = quest.questName;

            string objectiveSummary = "";
            foreach (var objective in quest.objectives)
            {
                if (objective is KillObjective killObjective)
                {
                    objectiveSummary += killObjective.description + " (" + killObjective.currentAmount + " / " + killObjective.requiredAmount + ")\n";
                }
                else if (objective is GoToObjective goToObjective)
                {
                    objectiveSummary += goToObjective.description + (goToObjective.isComplete ? " (Tamamlandý)" : " (Git)") + "\n";
                }
            }
            objectiveText.text = objectiveSummary;
        }
        else if (questPanel != null)
        {
            questPanel.SetActive(false);
        }
    }
}