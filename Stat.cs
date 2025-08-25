// Stat.cs (SON ve DOÐRU HALÝ)
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    // Inspector'da görünmesi için SerializeField kullanýyoruz,
    // ama baþka script'lerin doðrudan eriþmesini engellemek için private yapýyoruz.
    [SerializeField]
    private float baseValue;

    // Bu listeyi private yaparak dýþarýdan müdahaleyi engelliyoruz.
    private readonly List<float> modifiers = new List<float>();

    // Temel deðeri ve tüm bonuslarý toplayarak o anki nihai deðeri döndürür.
    public float GetValue()
    {
        float finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }
    // YENÝ FONKSÝYON: Save/Load için temel deðeri ayarlamamýzý saðlar.
    public void SetBaseValue(float maxHealth)
    {
        // Baþýna (int) ekleyerek "evet, bu ondalýklýyý tamsayýya çevirmek istediðimden eminim" diyoruz.
        baseValue = (int)maxHealth;
    }

    // Stat'a yeni bir bonus/modifier ekler.
    public void AddModifier(float modifier)
    {
        if (modifier != 0)
        {
            modifiers.Add(modifier);
        }
    }

    // Stat'tan bir bonusu/modifier'ý kaldýrýr.
    public void RemoveModifier(float modifier)
    {
        if (modifier != 0)
        {
            modifiers.Remove(modifier);
        }
    }
}