// Stat.cs (SON ve DO�RU HAL�)
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    // Inspector'da g�r�nmesi i�in SerializeField kullan�yoruz,
    // ama ba�ka script'lerin do�rudan eri�mesini engellemek i�in private yap�yoruz.
    [SerializeField]
    private float baseValue;

    // Bu listeyi private yaparak d��ar�dan m�dahaleyi engelliyoruz.
    private readonly List<float> modifiers = new List<float>();

    // Temel de�eri ve t�m bonuslar� toplayarak o anki nihai de�eri d�nd�r�r.
    public float GetValue()
    {
        float finalValue = baseValue;
        modifiers.ForEach(x => finalValue += x);
        return finalValue;
    }
    // YEN� FONKS�YON: Save/Load i�in temel de�eri ayarlamam�z� sa�lar.
    public void SetBaseValue(float maxHealth)
    {
        // Ba��na (int) ekleyerek "evet, bu ondal�kl�y� tamsay�ya �evirmek istedi�imden eminim" diyoruz.
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

    // Stat'tan bir bonusu/modifier'� kald�r�r.
    public void RemoveModifier(float modifier)
    {
        if (modifier != 0)
        {
            modifiers.Remove(modifier);
        }
    }
}