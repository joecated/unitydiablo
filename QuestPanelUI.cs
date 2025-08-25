// QuestPanelUI.cs (CANVAS GROUP FÝNAL VERSÝYON)
using UnityEngine;

public class QuestPanelUI : MonoBehaviour
{
    public static QuestPanelUI instance;
    public GameObject questPanel;
    public Transform activeQuestsContainer;
    public Transform completedQuestsContainer;
    public GameObject questEntryPrefab;

    private CanvasGroup canvasGroup;

    void Awake() { instance = this; canvasGroup = GetComponent<CanvasGroup>(); }
    void Start() { ClosePanel(); }

    public void TogglePanel()
    {
        bool isActive = canvasGroup.alpha > 0;
        if (isActive) { ClosePanel(); }
        else { OpenPanel(); }
    }

    // Bu public fonksiyonu QuestPanelUI.cs'in içine ekle
    public void ClosePanel()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void OpenPanel()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        RedrawPanels();
    }


    public void RedrawPanels()
    {
        foreach (Transform child in activeQuestsContainer) { Destroy(child.gameObject); }
        foreach (Transform child in completedQuestsContainer) { Destroy(child.gameObject); }
        foreach (Quest quest in QuestLog.instance.activeQuests)
        {
            GameObject entryObj = Instantiate(questEntryPrefab, activeQuestsContainer);
            entryObj.GetComponent<QuestUIEntry>().Setup(quest);
        }
        foreach (Quest quest in QuestLog.instance.completedQuests)
        {
            GameObject entryObj = Instantiate(questEntryPrefab, completedQuestsContainer);
            entryObj.GetComponent<QuestUIEntry>().Setup(quest);
        }
    }
}