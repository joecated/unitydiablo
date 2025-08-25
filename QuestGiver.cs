// QuestGiver.cs (Do�ru Hali)
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
            Debug.Log("Bu g�revi zaten ald�n.");
        }
    }

    public void QuestAccepted()
    {
        questAssigned = true;
    }
}