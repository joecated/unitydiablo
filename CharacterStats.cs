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

        // YENÝ: Eðer hasar türü "True" deðilse, zýrh hesaplamasý yap.
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

        // --- YENÝ KISIM: Hasar Yazýsýný Göster ---
        if (FloatingTextManager.instance != null)
        {
            FloatingTextManager.instance.ShowText(roundedDamage.ToString(), transform.position, Color.red);
        }
        // ------------------------------------

        ModifyHealth(-Mathf.RoundToInt(finalDamage));

        if (currentHealth <= 0)
        {
            // Bu objenin üzerindeki EnemyController script'ini bul ve Die() fonksiyonunu çaðýr.
            // Soru iþareti (?) null hatasý almaný engeller.
            GetComponent<EnemyStats>()?.Die();
        }
    }

    public float CalculateCooldown(float baseCooldown)
    {
        // Cooldown Reduction stat'ýnýn güncel deðerini alýyoruz.
        // Bu, zýrh, buff vb. her þeyden gelen toplam deðeri bize verir.
        float cdrValue = cooldownReduction.GetValue();

        // Formül: Nihai Süre = Temel Süre * (1 - (Azaltma Yüzdesi / 100))
        // Örnek: Temel Süre 10sn, CDR %20 ise -> 10 * (1 - 0.2) = 8sn.
        float finalCooldown = baseCooldown * (1 - (cdrValue / 100f));

        // Bekleme süresinin 0'ýn altýna düþmediðinden emin olalým.
        // Ýstersen buraya bir minimum süre de koyabilirsin (örn: 0.25f)
        if (finalCooldown < 0)
        {
            finalCooldown = 0;
        }

        return finalCooldown;
    }

    // YENÝ FONKSÝYON: Ýyileþtirme ve Yazýsýný Gösterme
    public void Heal(int amount)
    {
        // --- YENÝ KISIM: Ýyileþtirme Yazýsýný Göster ---
        if (FloatingTextManager.instance != null)
        {
            FloatingTextManager.instance.ShowText(amount.ToString(), transform.position, Color.green);
        }
        // ------------------------------------------
        ModifyHealth(amount);
    }
    public virtual void Die() { Debug.Log(transform.name + " öldü."); }
}