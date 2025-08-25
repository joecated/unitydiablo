// PlayerStats.cs (VFX SÝSTEMÝ EKLENMÝÞ FÝNAL VERSÝYON)
using UnityEngine;
using System;
using System.Collections;

public class PlayerStats : CharacterStats
{
    public static PlayerStats instance;

    #region Deðiþkenler
    [Header("Level & XP")]
    public int level = 1;
    public int currentXp = 0;
    public int xpToNextLevel = 100;

    [Header("Para Birimi")]
    public int gold = 0;

    [Header("Efektler")]
    public GameObject levelUpVFX;

    [Header("Bariyer VFX Referanslarý")]
    public GameObject barrierReadyVFX;
    public GameObject barrierTriggeredVFX;

    

    private GameObject readyVFXInstance;
    private GameObject triggeredVFXInstance;

    [Header("Bariyer Sistemi")]
    public float barrierCooldown = 45f;
    private bool hasBarrierItem = false;
    private bool isBarrierReady = true;
    private bool isImmuneFromBarrier = false;
    private float currentBarrierCooldown = 0f;

    public event Action OnStatsChanged;
    public event Action<int> OnLevelChanged;
    public event Action<int, int> OnXpChanged;
    public event Action<int> OnGoldChanged;
    public event Action<bool> OnBarrierStateChanged;
    public event Action<float, float> OnBarrierCooldownUpdated;
    #endregion

    public override void Awake()
    {
        base.Awake();
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);

        if (EquipmentManager.instance != null)
        {
            EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        }
    }

    void Update()
    {
        if (currentBarrierCooldown > 0)
        {
            currentBarrierCooldown -= Time.deltaTime;
            OnBarrierCooldownUpdated?.Invoke(currentBarrierCooldown, barrierCooldown);

            if (currentBarrierCooldown <= 0)
            {
                currentBarrierCooldown = 0;
                isBarrierReady = true;
                OnBarrierStateChanged?.Invoke(true);
                Debug.Log("Bariyer tekrar hazýr!");

                // YENÝ: Cooldown bitince "Hazýr" efektini yarat
                if (readyVFXInstance == null && hasBarrierItem)
                {
                    readyVFXInstance = Instantiate(barrierReadyVFX, transform.position, transform.rotation, transform);
                }
            }
        }
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        BarrierUI barrierUI = FindObjectOfType<BarrierUI>(true);
        if (barrierUI == null)
        {
            Debug.LogWarning("Sahnede BarrierUI bulunamadý.");
        }

        // --- ESKÝ EÞYAYI ÇIKARMA MANTIÐI ---
        if (oldItem != null && oldItem.grantsBarrierEffect)
        {
            hasBarrierItem = false;
            if (barrierUI != null) barrierUI.Hide();

            // YENÝ: Sadece tetiklenmiþi deðil, SAHNEDEKÝ TÜM bariyer efektlerini yok et.
            if (readyVFXInstance != null) Destroy(readyVFXInstance);
            if (triggeredVFXInstance != null) Destroy(triggeredVFXInstance);
        }

        // --- YENÝ EÞYAYI GÝYME MANTIÐI ---
        if (newItem != null && newItem.grantsBarrierEffect)
        {
            hasBarrierItem = true;
            this.barrierCooldown = newItem.barrierEffectCooldown;

            if (barrierUI != null)
            {
                barrierUI.Show();
                barrierUI.UpdateState(isBarrierReady);
            }

            // YENÝ: Eðer eþyayý giydiðimizde bariyer ZATEN HAZIRSA, "Hazýr" efektini anýnda yarat.
            if (isBarrierReady && readyVFXInstance == null)
            {
                readyVFXInstance = Instantiate(barrierReadyVFX, transform.position, transform.rotation, transform);
            }
        }

        // --- Diðer Stat Güncellemeleri ---
        ModifyHealth(0);
        ModifyMana(0);
        OnStatsChanged?.Invoke();
    }

    public override void TakeDamage(int damageAmount, DamageType type)
    {
        if (isImmuneFromBarrier)
        {
            return;
        }

        if (hasBarrierItem && isBarrierReady)
        {
            isBarrierReady = false;
            currentBarrierCooldown = barrierCooldown;
            OnBarrierStateChanged?.Invoke(false);
            StartCoroutine(BarrierImmunityCoroutine());

            if (FloatingTextManager.instance != null)
                FloatingTextManager.instance.ShowText("BARÝYER!", transform.position, Color.cyan);

            // YENÝ: "Hazýr" efektini yok et, "Tetiklendi" efektini yarat
            if (readyVFXInstance != null) Destroy(readyVFXInstance);
            triggeredVFXInstance = Instantiate(barrierTriggeredVFX, transform.position, transform.rotation, transform);

            return;
        }

        base.TakeDamage(damageAmount, type);
    }

    IEnumerator BarrierImmunityCoroutine()
    {
        isImmuneFromBarrier = true;
        yield return new WaitForSeconds(2f);
        isImmuneFromBarrier = false;

        // YENÝ: 2 saniyelik dokunulmazlýk bitince "Tetiklendi" efektini yok et
        if (triggeredVFXInstance != null) Destroy(triggeredVFXInstance);
    }

    #region Diðer Fonksiyonlar
    public void AddGold(int amount) { gold += amount; OnGoldChanged?.Invoke(gold); }
    public bool SpendGold(int amount) { if (gold >= amount) { gold -= amount; OnGoldChanged?.Invoke(gold); return true; } else { return false; } }
    public void Start() { OnLevelChanged?.Invoke(level); OnXpChanged?.Invoke(currentXp, xpToNextLevel); OnGoldChanged?.Invoke(gold); }
    public void SpendMana(int amount) { ModifyMana(-amount); }
    public void GainXp(int amount) { currentXp += amount; while (currentXp >= xpToNextLevel) { currentXp -= xpToNextLevel; LevelUp(); } OnXpChanged?.Invoke(currentXp, xpToNextLevel); }
    void LevelUp()
    {
        level++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);

        // Her seviyede maksimum caný 10, manayý 5 artýr
        maxHealth.AddModifier(50);
        maxMana.AddModifier(12);

        // --- HATA DÜZELTMESÝ & YENÝLÝK ---

        // 1. Anlýk can ve manayý tamamen doldur.
        currentHealth = (int)maxHealth.GetValue();
        currentMana = (int)maxMana.GetValue();

        // 2. UI'ý güncellemek için event'leri direkt çaðýrmak yerine, ana fonksiyonlarý çaðýr.
        ModifyHealth(0); // Bu, OnHealthChanged sinyalini güvenli bir þekilde tetikler.
        ModifyMana(0);   // Bu da OnManaChanged sinyalini güvenli bir þekilde tetikler.
        OnLevelChanged?.Invoke(level);

        // 3. Seviye atlama efektini (VFX) yarat.
        // (Bunun için script'in en üstüne public GameObject levelUpVFX; ekleyeceðiz)
        if (levelUpVFX != null)
        {
            // Efekti karakterin pozisyonunda yarat ve karakterin çocuðu yap ki onunla hareket etsin.
            Instantiate(levelUpVFX, transform.position, Quaternion.identity, transform);
        }

        // 4. "Seviye Atlandý!" yazýsýný çýkar.
        if (FloatingTextManager.instance != null)
        {
            // Yazýnýn pozisyonu için karakterin merkezini alalým (vücudunun ortasý)
            Vector3 textPosition = transform.position + Vector3.up; // 1 birim yukarý
            FloatingTextManager.instance.ShowText("Seviye Atlandý!", textPosition, Color.yellow);
        }
        // --- GÜNCELLEME BÝTTÝ ---

        Debug.Log("SEVÝYE ATLANDI! Yeni Seviye: " + level);
    }
    #endregion
}