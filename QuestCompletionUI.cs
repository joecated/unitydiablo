// QuestCompletionUI.cs
using UnityEngine;
using TMPro;
using System.Collections;
using System.Text; // StringBuilder için gerekli

public class QuestCompletionUI : MonoBehaviour
{
    [Header("UI Referanslarý")]
    public GameObject questCompletePanel;
    public TextMeshProUGUI titleText; // "Görev Tamamlandý!" yazýsý için
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI rewardsText;

    [Header("Ayarlar")]
    public float displayDuration = 5f; // Panel ekranda kaç saniye kalacak?

    void Start()
    {
        // QuestLog'un sinyalini dinlemeye baþla
        if (QuestLog.instance != null)
        {
            QuestLog.instance.OnQuestCompleted += ShowQuestCompletion;
        }

        // Paneli oyun baþýnda gizle
        questCompletePanel.SetActive(false);
    }

    void OnDestroy()
    {
        // Dinlemeyi býrak (sahne deðiþimi vb. durumlarda hata almamak için)
        if (QuestLog.instance != null)
        {
            QuestLog.instance.OnQuestCompleted -= ShowQuestCompletion;
        }
    }

    private void ShowQuestCompletion(Quest completedQuest)
    {
        // Metinleri doldur
        questNameText.text = completedQuest.questName;

        // Ödül metnini dinamik olarak oluþtur
        StringBuilder rewardsBuilder = new StringBuilder();
        rewardsBuilder.AppendLine("Ödüller:");

        if (completedQuest.experienceReward > 0)
        {
            rewardsBuilder.AppendLine("- " + completedQuest.experienceReward + " Tecrübe Puaný");
        }
        if (completedQuest.goldReward > 0)
        {
            rewardsBuilder.AppendLine("- " + completedQuest.goldReward + " Altýn");
        }
        // Not: Eþya ödülü için de benzer bir kontrol ekleyebilirsin
        // if (completedQuest.itemReward != null) { ... }

        rewardsText.text = rewardsBuilder.ToString();

        // Paneli göster ve bir süre sonra gizle
        StartCoroutine(ShowAndHidePanel());
    }

    private IEnumerator ShowAndHidePanel()
    {
        questCompletePanel.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        questCompletePanel.SetActive(false);
    }
}