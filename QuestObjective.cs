// QuestObjective.cs
using UnityEngine;

// Bu ana s�n�f, Unity edit�r�nde do�rudan kullan�lamaz, sadece miras al�n�r.
public abstract class QuestObjective : ScriptableObject
{
    [TextArea]
    public string description;
    public bool isComplete = false;

    // Her hedef t�r�, tamamlan�p tamamlanmad���n� kendi kontrol edecek.
    public abstract bool CheckProgress();
}