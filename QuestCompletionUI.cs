// QuestCompletionUI.cs
using UnityEngine;
using TMPro;
using System.Collections;
using System.Text; // StringBuilder i�in gerekli

public class QuestCompletionUI : MonoBehaviour
{
    [Header("UI Referanslar�")]
    public GameObject questCompletePanel;
    public TextMeshProUGUI titleText; // "G�rev Tamamland�!" yaz�s� i�in
    public TextMeshProUGUI questNameText;
    public TextMeshProUGUI rewardsText;

    [Header("Ayarlar")]
    public float displayDuration = 5f; // Panel ekranda ka� saniye kalacak?

    void Start()
    {
        // QuestLog'un sinyalini dinlemeye ba�la
        if (QuestLog.instance != null)
        {
            QuestLog.instance.OnQuestCompleted += ShowQuestCompletion;
        }

        // Paneli oyun ba��nda gizle
        questCompletePanel.SetActive(false);
    }

    void OnDestroy()
    {
        // Dinlemeyi b�rak (sahne de�i�imi vb. durumlarda hata almamak i�in)
        if (QuestLog.instance != null)
        {
            QuestLog.instance.OnQuestCompleted -= ShowQuestCompletion;
        }
    }

    private void ShowQuestCompletion(Quest completedQuest)
    {
        // Metinleri doldur
        questNameText.text = completedQuest.questName;

        // �d�l metnini dinamik olarak olu�tur
        StringBuilder rewardsBuilder = new StringBuilder();
        rewardsBuilder.AppendLine("�d�ller:");

        if (completedQuest.experienceReward > 0)
        {
            rewardsBuilder.AppendLine("- " + completedQuest.experienceReward + " Tecr�be Puan�");
        }
        if (completedQuest.goldReward > 0)
        {
            rewardsBuilder.AppendLine("- " + completedQuest.goldReward + " Alt�n");
        }
        // Not: E�ya �d�l� i�in de benzer bir kontrol ekleyebilirsin
        // if (completedQuest.itemReward != null) { ... }

        rewardsText.text = rewardsBuilder.ToString();

        // Paneli g�ster ve bir s�re sonra gizle
        StartCoroutine(ShowAndHidePanel());
    }

    private IEnumerator ShowAndHidePanel()
    {
        questCompletePanel.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        questCompletePanel.SetActive(false);
    }
}