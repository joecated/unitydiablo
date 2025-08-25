// QuestGiver.cs (Doðru Hali)
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    public Dialogue dialogue;
    public Quest quest;
    private bool questAssigned = false;

    private void OnMouseDown()
    {
        if (!questAssigned && quest != null && dialogue != null)
        {
            DialogueManager.instance.StartDialogue(dialogue, quest, this);
        }
        else if (questAssigned)
        {
            Debug.Log("Bu görevi zaten aldýn.");
        }
    }

    public void QuestAccepted()
    {
        questAssigned = true;
    }
}