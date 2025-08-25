// DialogueManager.cs
using UnityEngine;
using TMPro; // TextMeshPro kullanacaðýz
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("UI Elementleri")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI sentenceText;
    public GameObject continueButton;

    [Header("Görev Teklif UI")]
    public GameObject questOfferPanel;
    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescriptionText;
    public TextMeshProUGUI questRewardsText;
    public GameObject acceptButton;
    public GameObject rejectButton;

    private Queue<DialogueLine> dialogueQueue;
    private Quest questToOffer;
    private QuestGiver currentQuestGiver;

    void Awake()
    {
        instance = this;
        dialogueQueue = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue, Quest quest, QuestGiver questGiver)
    {
        questToOffer = quest;
        currentQuestGiver = questGiver;

        dialoguePanel.SetActive(true);
        questOfferPanel.SetActive(false);

        dialogueQueue.Clear();

        foreach (DialogueLine line in dialogue.lines)
        {
            dialogueQueue.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = dialogueQueue.Dequeue();
        speakerNameText.text = currentLine.speakerName;
        sentenceText.text = currentLine.sentence;
    }

    void EndDialogue()
    {
        if (questToOffer != null)
        {
            dialoguePanel.SetActive(false);
            questOfferPanel.SetActive(true);

            questTitleText.text = questToOffer.questName;
            questDescriptionText.text = questToOffer.description;

            // HATA DÜZELTMESÝ: Ödül bilgilerini doðrudan Quest'in içinden al.
            questRewardsText.text = "Ödüller:\n" +
                                    "- " + questToOffer.experienceReward + " EXP\n" +
                                    "- " + questToOffer.goldReward + " Altýn";
        }
        else
        {
            CloseAllPanels();
        }
    }

    public void OnAcceptQuest()
    {
        FindObjectOfType<QuestLog>().AddQuest(questToOffer);
        currentQuestGiver.QuestAccepted();
        CloseAllPanels();
    }

    public void OnRejectQuest()
    {
        CloseAllPanels();
    }

    private void CloseAllPanels()
    {
        dialoguePanel.SetActive(false);
        questOfferPanel.SetActive(false);
    }
}