// PlayerStats.cs (VFX S�STEM� EKLENM�� F�NAL VERS�YON)
using UnityEngine;
using System;
using System.Collections;

public class PlayerStats : CharacterStats
{
    public static PlayerStats instance;

    #region De�i�kenler
    [Header("Level & XP")]
    public int level = 1;
    public int currentXp = 0;
    public int xpToNextLevel = 100;

    [Header("Para Birimi")]
    public int gold = 0;

    [Header("Efektler")]
    public GameObject levelUpVFX;

    [Header("Bariyer VFX Referanslar�")]
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
                Debug.Log("Bariyer tekrar haz�r!");

                // YEN�: Cooldown bitince "Haz�r" efektini yarat
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
            Debug.LogWarning("Sahnede BarrierUI bulunamad�.");
        }

        // --- ESK� E�YAYI �IKARMA MANTI�I ---
        if (oldItem != null && oldItem.grantsBarrierEffect)
        {
            hasBarrierItem = false;
            if (barrierUI != null) barrierUI.Hide();

            // YEN�: Sadece tetiklenmi�i de�il, SAHNEDEK� T�M bariyer efektlerini yok et.
            if (readyVFXInstance != null) Destroy(readyVFXInstance);
            if (triggeredVFXInstance != null) Destroy(triggeredVFXInstance);
        }

        // --- YEN� E�YAYI G�YME MANTI�I ---
        if (newItem != null && newItem.grantsBarrierEffect)
        {
            hasBarrierItem = true;
            this.barrierCooldown = newItem.barrierEffectCooldown;

            if (barrierUI != null)
            {
                barrierUI.Show();
                barrierUI.UpdateState(isBarrierReady);
            }

            // YEN�: E�er e�yay� giydi�imizde bariyer ZATEN HAZIRSA, "Haz�r" efektini an�nda yarat.
            if (isBarrierReady && readyVFXInstance == null)
            {
                readyVFXInstance = Instantiate(barrierReadyVFX, transform.position, transform.rotation, transform);
            }
        }

        // --- Di�er Stat G�ncellemeleri ---
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
                FloatingTextManager.instance.ShowText("BAR�YER!", transform.position, Color.cyan);

            // YEN�: "Haz�r" efektini yok et, "Tetiklendi" efektini yarat
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

        // YEN�: 2 saniyelik dokunulmazl�k bitince "Tetiklendi" efektini yok et
        if (triggeredVFXInstance != null) Destroy(triggeredVFXInstance);
    }

    #region Di�er Fonksiyonlar
    public void AddGold(int amount) { gold += amount; OnGoldChanged?.Invoke(gold); }
    public bool SpendGold(int amount) { if (gold >= amount) { gold -= amount; OnGoldChanged?.Invoke(gold); return true; } else { return false; } }
    public void Start() { OnLevelChanged?.Invoke(level); OnXpChanged?.Invoke(currentXp, xpToNextLevel); OnGoldChanged?.Invoke(gold); }
    public void SpendMana(int amount) { ModifyMana(-amount); }
    public void GainXp(int amount) { currentXp += amount; while (currentXp >= xpToNextLevel) { currentXp -= xpToNextLevel; LevelUp(); } OnXpChanged?.Invoke(currentXp, xpToNextLevel); }
    void LevelUp()
    {
        level++;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f);

        // Her seviyede maksimum can� 10, manay� 5 art�r
        maxHealth.AddModifier(50);
        maxMana.AddModifier(12);

        // --- HATA D�ZELTMES� & YEN�L�K ---

        // 1. Anl�k can ve manay� tamamen doldur.
        currentHealth = (int)maxHealth.GetValue();
        currentMana = (int)maxMana.GetValue();

        // 2. UI'� g�ncellemek i�in event'leri direkt �a��rmak yerine, ana fonksiyonlar� �a��r.
        ModifyHealth(0); // Bu, OnHealthChanged sinyalini g�venli bir �ekilde tetikler.
        ModifyMana(0);   // Bu da OnManaChanged sinyalini g�venli bir �ekilde tetikler.
        OnLevelChanged?.Invoke(level);

        // 3. Seviye atlama efektini (VFX) yarat.
        // (Bunun i�in script'in en �st�ne public GameObject levelUpVFX; ekleyece�iz)
        if (levelUpVFX != null)
        {
            // Efekti karakterin pozisyonunda yarat ve karakterin �ocu�u yap ki onunla hareket etsin.
            Instantiate(levelUpVFX, transform.position, Quaternion.identity, transform);
        }

        // 4. "Seviye Atland�!" yaz�s�n� ��kar.
        if (FloatingTextManager.instance != null)
        {
            // Yaz�n�n pozisyonu i�in karakterin merkezini alal�m (v�cudunun ortas�)
            Vector3 textPosition = transform.position + Vector3.up; // 1 birim yukar�
            FloatingTextManager.instance.ShowText("Seviye Atland�!", textPosition, Color.yellow);
        }
        // --- G�NCELLEME B�TT� ---

        Debug.Log("SEV�YE ATLANDI! Yeni Seviye: " + level);
    }
    #endregion
}