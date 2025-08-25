// CharacterStats.cs
using UnityEngine;
using System;

public class CharacterStats : MonoBehaviour
{
    [Header("Temel Kaynaklar")]
    public Stat maxHealth;
    public int currentHealth { get; protected set; }
    public Stat maxMana;
    public int currentMana { get; protected set; }

    [Header("Statlar")]
    public Stat damage;
    public Stat armor;
    public Stat magicResist;
    public Stat abilityPower;
    public Stat critChance;
    public Stat critDamage;
    public Stat cooldownReduction;

    public event Action<int, int> OnHealthChanged;
    public event Action<int, int> OnManaChanged;

    public virtual void Awake()
    {
        currentHealth = (int)maxHealth.GetValue();
        currentMana = (int)maxMana.GetValue();
    }

    public void ModifyHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, (int)maxHealth.GetValue());
        OnHealthChanged?.Invoke(currentHealth, (int)maxHealth.GetValue());
    }

    public void ModifyMana(int amount)
    {
        currentMana += amount;
        currentMana = Mathf.Clamp(currentMana, 0, (int)maxMana.GetValue());
        OnManaChanged?.Invoke(currentMana, (int)maxMana.GetValue());
    }



    public virtual void TakeDamage(int damageAmount, DamageType type)
    {
        float finalDamage = damageAmount;

        // YEN�: E�er hasar t�r� "True" de�ilse, z�rh hesaplamas� yap.
        if (type != DamageType.True)
        {
            if (type == DamageType.Physical)
                finalDamage = damageAmount * (100f / (100f + armor.GetValue()));
            else if (type == DamageType.Magical)
                finalDamage = damageAmount * (100f / (100f + magicResist.GetValue()));
        }

        if (type == DamageType.Physical)
            finalDamage = damageAmount * (100f / (100f + armor.GetValue()));
        else if (type == DamageType.Magical)
            finalDamage = damageAmount * (100f / (100f + magicResist.GetValue()));


        int roundedDamage = Mathf.RoundToInt(finalDamage);

        // --- YEN� KISIM: Hasar Yaz�s�n� G�ster ---
        if (FloatingTextManager.instance != null)
        {
            FloatingTextManager.instance.ShowText(roundedDamage.ToString(), transform.position, Color.red);
        }
        // ------------------------------------

        ModifyHealth(-Mathf.RoundToInt(finalDamage));

        if (currentHealth <= 0)
        {
            // Bu objenin �zerindeki EnemyController script'ini bul ve Die() fonksiyonunu �a��r.
            // Soru i�areti (?) null hatas� alman� engeller.
            GetComponent<EnemyStats>()?.Die();
        }
    }

    public float CalculateCooldown(float baseCooldown)
    {
        // Cooldown Reduction stat'�n�n g�ncel de�erini al�yoruz.
        // Bu, z�rh, buff vb. her �eyden gelen toplam de�eri bize verir.
        float cdrValue = cooldownReduction.GetValue();

        // Form�l: Nihai S�re = Temel S�re * (1 - (Azaltma Y�zdesi / 100))
        // �rnek: Temel S�re 10sn, CDR %20 ise -> 10 * (1 - 0.2) = 8sn.
        float finalCooldown = baseCooldown * (1 - (cdrValue / 100f));

        // Bekleme s�resinin 0'�n alt�na d��medi�inden emin olal�m.
        // �stersen buraya bir minimum s�re de koyabilirsin (�rn: 0.25f)
        if (finalCooldown < 0)
        {
            finalCooldown = 0;
        }

        return finalCooldown;
    }

    // YEN� FONKS�YON: �yile�tirme ve Yaz�s�n� G�sterme
    public void Heal(int amount)
    {
        // --- YEN� KISIM: �yile�tirme Yaz�s�n� G�ster ---
        if (FloatingTextManager.instance != null)
        {
            FloatingTextManager.instance.ShowText(amount.ToString(), transform.position, Color.green);
        }
        // ------------------------------------------
        ModifyHealth(amount);
    }
    public virtual void Die() { Debug.Log(transform.name + " �ld�."); }
}