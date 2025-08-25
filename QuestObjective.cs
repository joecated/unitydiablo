// QuestObjective.cs
using UnityEngine;

// Bu ana sýnýf, Unity editöründe doðrudan kullanýlamaz, sadece miras alýnýr.
public abstract class QuestObjective : ScriptableObject
{
    [TextArea]
    public string description;
    public bool isComplete = false;

    // Her hedef türü, tamamlanýp tamamlanmadýðýný kendi kontrol edecek.
    public abstract bool CheckProgress();
}