// PlayerCombat.cs (DASH HATASI DÜZELTÝLDÝ)
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq; // Bu satýrý en üste ekle

public class PlayerCombat : MonoBehaviour
{

    public static PlayerCombat instance; // YENÝ: Singleton referansý

    // YENÝ EVENT'LER
    public event Action<bool> OnSpiritFormStateChanged;
    public event Action<float, float> OnSpiritFormDurationUpdated;

    void Awake() // YENÝ
    {
        instance = this;
    }



    // ... (En üstteki deðiþkenlerin hepsi ayný kalýyor) ...
    #region Deðiþkenler
    // --- RUH BÝÇEN SÝSTEMÝ DEÐÝÞKENLERÝ ---
    private bool isSpiritActive = false;
    private float spiritEndTime;
    private Vector3 bodyPosition;
    private GameObject bodyInstance;
    private LineRenderer tetherInstance;
    private float totalDamageDealtInSpiritForm;
    private List<CharacterStats> damagedEnemies = new List<CharacterStats>();
    private SpiritSkill activeSpiritSkill;
    private GameObject spiritVFXInstance;
    // --- Bitti ---

    [Header("Yetenekler")]
    public Skill basicAttackSkill;
    public Skill[] skills = new Skill[4];
    [Header("Referanslar")]
    public Transform vfxSpawnPoint;
    public bool IsQComboActive { get; private set; } = false;
    public event Action<int, float, float> OnSkillCooldownUpdate;
    public event Action<int, Skill> OnSkillChanged;
    public event Action<float, float> OnQComboTimerUpdate;
    public event Action<bool> OnPassiveStateChanged;
    private Dictionary<Skill, float> skillCooldowns = new Dictionary<Skill, float>();
    public bool IsComboing { get; private set; } = false;
    private ComboSkill currentQCombo;
    private int qComboCounter = 0;
    private float lastQPressTime = 0f;
    private bool isUltimateActive = false;
    private float ultimateEndTime = 0f;
    private BuffSkill activeBuffSkill;
    private bool isPassiveCharged = false;
    #endregion

    // --- YENÝ EKLENEN REFERANS ---
    private CharacterController characterController;
    // ----------------------------

    // Gerekli Diðer Bileþenler
    private PlayerStats playerStats;
    public Animator animator; // Bu public olmalý ki PlayerAnimator eriþebilsin
    private PlayerController playerController;
    private NavMeshAgent agent;

    void Start()
    {
        // ... (Start fonksiyonun ayný, sadece animator ve characterController referanslarýný düzeltelim) ...
        playerStats = GetComponent<PlayerStats>();
        playerController = GetComponent<PlayerController>();
        agent = GetComponent<NavMeshAgent>();
        characterController = GetComponentInParent<PlayerController>().characterController;
        animator = GetComponentInParent<PlayerController>().animator;
        // ... (geri kalaný ayný)

        // ... (Start fonksiyonunun geri kalaný ayný) ...
        #region Start Geri Kalan
        if (basicAttackSkill != null)
        {
            skillCooldowns[basicAttackSkill] = 0f;
        }
        foreach (Skill skill in skills)
        {
            if (skill != null)
            {
                skillCooldowns[skill] = 0f;
            }
        }
        #endregion
    }

    // --- DASH FONKSÝYONU GÜNCELLENDÝ ---
    IEnumerator Dash(float duration, float speed)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            // Hareketi NavMeshAgent'a deðil, doðrudan CharacterController'a yaptýrýyoruz.
            // transform.forward yerine characterController.transform.forward kullanmak daha güvenli
            Vector3 movement = characterController.transform.forward * speed * Time.deltaTime;
            characterController.Move(movement);
            yield return null;
        }
    }

    public bool IsSpiritActive()
    {
        return isSpiritActive;
    }

    public float GetStoredSpiritDamage()
    {
        return totalDamageDealtInSpiritForm;
    }

    // Fonksiyon artýk hangi yetenek için hesaplama yapacaðýný parametre olarak alýyor
    public float GetCurrentSpiritDamagePercent(SpiritSkill spiritSkill)
    {
        // Güvenlik kontrolü: Eðer bir sebepten ötürü yetenek boþ gelirse, hata vermesin.
        if (spiritSkill == null) return 0f;

        float levelProgress = (playerStats.level - 1) / 99f;

        // Artýk o an aktif olaný deðil, kendisine verilen yeteneðin deðerlerini kullanýyor
        return Mathf.Lerp(spiritSkill.minDamageReturnPercent, spiritSkill.maxDamageReturnPercent, levelProgress) / 100f;
    }

    // --- GERÝ KALAN TÜM FONKSÝYONLAR BÝREBÝR AYNI ---
    #region Dokunulmayan Fonksiyonlar
    void Update()
    {
        if (isSpiritActive)
        {
            float remainingTime = spiritEndTime - Time.time;
            OnSpiritFormDurationUpdated?.Invoke(remainingTime, activeSpiritSkill.duration);

            if (remainingTime <= 0)
            {
                EndSpiritForm();
            }
        }

        // --- RUH BÝÇEN SÝSTEMÝ: Zamanlayýcý Kontrolü ---
        if (isSpiritActive && Time.time >= spiritEndTime)
        {
            EndSpiritForm();
        }
        // --- Bitti ---


        UpdateCooldowns();
        HandleInput();
        if (qComboCounter > 0)
        {
            float timeSinceLastQ = Time.time - lastQPressTime;
            float remainingTime = currentQCombo.comboResetTime - timeSinceLastQ;
            OnQComboTimerUpdate?.Invoke(remainingTime, currentQCombo.comboResetTime);
            if (remainingTime <= 0)
            {
                ResetQCombo();
            }
        }
        if (isUltimateActive && Time.time > ultimateEndTime)
        {
            DeactivateUltimate();
        }
    }

    // YENÝ: Bu fonksiyonu Update'in SONUNA ekle
    void LateUpdate()
    {
        // Eðer ruh formundaysak, baðýn pozisyonlarýný her saniye güncelle
        if (isSpiritActive && tetherInstance != null)
        {
            tetherInstance.SetPosition(0, bodyPosition); // 1. nokta: Beden
            tetherInstance.SetPosition(1, transform.position); // 2. nokta: Ruh (yani biz)
        }
    }

    // Hasar veren tüm fonksiyonlarý, ruh formunda hasar biriktirecek þekilde güncelliyoruz
    private void RecordDamage(CharacterStats enemy, int damage)
    {
        if (isSpiritActive)
        {
            totalDamageDealtInSpiritForm += damage;
            // Eðer bu düþmaný daha önce listeye eklemediysek, ekle
            if (!damagedEnemies.Contains(enemy))
            {
                damagedEnemies.Add(enemy);
            }
            Debug.Log("Ruh Formunda Hasar Birikti: " + totalDamageDealtInSpiritForm);
        }
    }

    void UpdateCooldowns()
    {
        Skill qSkill = skills[0];
        List<Skill> skillsToUpdate = new List<Skill>(skillCooldowns.Keys);
        foreach (Skill skill in skillsToUpdate)
        {
            if (skillCooldowns.ContainsKey(skill) && skillCooldowns[skill] > 0)
            {
                skillCooldowns[skill] -= Time.deltaTime;
                if (skill == qSkill && IsQComboActive)
                {
                    continue;
                }
                for (int i = 0; i < skills.Length; i++)
                {
                    if (skills[i] == skill || (activeBuffSkill != null && activeBuffSkill.secondCastSkill == skill))
                    {
                        OnSkillCooldownUpdate?.Invoke(i, skillCooldowns[skill], skill.cooldown);
                    }
                }
            }
        }
    }
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Q)) UseSkill(0);
        if (Input.GetKeyDown(KeyCode.W)) UseSkill(1);
        if (Input.GetKeyDown(KeyCode.E)) UseSkill(2);
        if (Input.GetKeyDown(KeyCode.R)) UseSkill(3);
    }
    private void UseSkill(int slotIndex)
    {
        if (slotIndex >= skills.Length || skills[slotIndex] == null) return;
        if (isUltimateActive && slotIndex == 3)
        {
            activeBuffSkill.secondCastSkill.Activate(this);
            DeactivateUltimate();
            return;
        }
        Skill skillToUse = skills[slotIndex];
        bool isOffCooldown = skillCooldowns.ContainsKey(skillToUse) && skillCooldowns[skillToUse] <= 0;
        bool canContinueCombo = (slotIndex == 0 && IsQComboActive);
        if ((isOffCooldown || canContinueCombo) && playerStats.currentMana >= skillToUse.manaCost)
        {
            playerStats.SpendMana(skillToUse.manaCost);
            skillToUse.Activate(this);
            isPassiveCharged = true;
            OnPassiveStateChanged?.Invoke(true);
            Debug.Log("Pasif Yüklendi!");
            if (skillToUse.skillType != SkillType.Combo && skillToUse.skillType != SkillType.Buff)
            {
                float finalCooldown = playerStats.CalculateCooldown(skillToUse.cooldown);
                
                Debug.Log(skillToUse.name + " kullanýldý. Temel CD: " + skillToUse.cooldown + "sn. Ýndirimli CD: " + finalCooldown + "sn.");
            }
        }
    }
    public void DoBasicAttack()
    {
        if (basicAttackSkill != null && skillCooldowns.ContainsKey(basicAttackSkill) && skillCooldowns[basicAttackSkill] <= 0)
        {
            basicAttackSkill.Activate(this);
            skillCooldowns[basicAttackSkill] = playerStats.CalculateCooldown(basicAttackSkill.cooldown);
        }
    }
    // Bu fonksiyon artýk bir parametreye ihtiyaç duymuyor, E yeteneðini kendi buluyor.
    public int GetCurrentPassiveBonusDamage()
    {
        // E yeteneði slot 2'de (0=Q, 1=W, 2=E)
        if (skills[2] is SpiritSkill spiritSkill)
        {
            float levelProgress = (playerStats.level - 1) / 99f;
            float percent = Mathf.Lerp(spiritSkill.minDamageReturnPercent, spiritSkill.maxDamageReturnPercent, levelProgress) / 100f;
            return Mathf.RoundToInt(playerStats.damage.GetValue() * percent);
        }
        return 0; // Eðer E slotunda SpiritSkill yoksa, bonus hasar 0'dýr.
    }
    public void DoDamageSkill(Skill skill)
    {
        Transform target = playerController.GetTarget();
        if (target == null) return;

        // --- Hasar Hesaplama Kýsmý (Ayný) ---
        int baseDamage = Mathf.RoundToInt(playerStats.damage.GetValue()) + skill.damage;
        if (skill.apScaling > 0)
        {
            int apBonusDamage = Mathf.RoundToInt(playerStats.abilityPower.GetValue() * skill.apScaling);
            baseDamage += apBonusDamage;
        }
        if (skill == basicAttackSkill && isPassiveCharged)
        {
            int bonusDamage = GetCurrentPassiveBonusDamage();
            baseDamage += bonusDamage;
            isPassiveCharged = false;
            OnPassiveStateChanged?.Invoke(false);
        }
        float critChance = playerStats.critChance.GetValue();
        if (UnityEngine.Random.Range(0f, 1f) <= critChance)
        {
            float critDamageMultiplier = playerStats.critDamage.GetValue();
            baseDamage = Mathf.RoundToInt(baseDamage * critDamageMultiplier);
        }
        // --- Hesaplama Bitti ---

        // Hedefe hasarý uygula
        CharacterStats targetStats = target.GetComponent<CharacterStats>();
        if (targetStats != null)
        {
            targetStats.TakeDamage(baseDamage, skill.damageType);

            // YENÝ VE EN ÖNEMLÝ KISIM: Hasarý kumbaraya kaydet!
            RecordDamage(targetStats, baseDamage);
        }

        // Animasyonu tetikle
        animator.SetTrigger("Attack");
    }
    public void DoComboSkill(ComboSkill skill)
    {
        if (Time.time - lastQPressTime > skill.comboResetTime || qComboCounter >= skill.comboSteps.Length)
        {
            qComboCounter = 0;
        }
        if (qComboCounter == 0)
        {
            skillCooldowns[skill] = playerStats.CalculateCooldown(skill.cooldown);
            IsQComboActive = true;
        }
        currentQCombo = skill;
        lastQPressTime = Time.time;
        ComboStep currentStep = skill.comboSteps[qComboCounter];
        animator.SetInteger("Q_State", qComboCounter + 1);
        StartCoroutine(Dash(0.1f, currentStep.dashForce));
        Collider[] hits = Physics.OverlapSphere(transform.position + transform.forward, currentStep.damageRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                int totalDamage = Mathf.RoundToInt(playerStats.damage.GetValue()) + currentStep.damage;
                hit.GetComponent<CharacterStats>()?.TakeDamage(totalDamage, DamageType.Physical);
                RecordDamage(hit.GetComponent<CharacterStats>(), totalDamage); // YENÝ: Hasarý kaydet
            }
        }
        qComboCounter++;
        if (qComboCounter >= skill.comboSteps.Length)
        {
            StartCoroutine(DelayedResetQCombo());
        }
    }

    // E yeteneði çaðrýldýðýnda
    public void DoSpiritSkill(SpiritSkill skill)
    {
        if (!isSpiritActive)
        {
            BeginSpiritForm(skill);
        }
        else
        {
            EndSpiritForm();
        }
    }
    void BeginSpiritForm(SpiritSkill skill)
    {
        activeSpiritSkill = skill;
        isSpiritActive = true;
        spiritEndTime = Time.time + skill.duration;
        totalDamageDealtInSpiritForm = 0f;
        damagedEnemies.Clear();

        

        // Bedenini geride býrak
        bodyPosition = transform.position;
        if (skill.bodyPrefab != null)
        {
            bodyInstance = Instantiate(skill.bodyPrefab, bodyPosition, transform.rotation);
        }

        // Ruh efektini aktif et
        if (skill.spiritVFX != null)
        {
            spiritVFXInstance = Instantiate(skill.spiritVFX, transform);
        }
        if (skill.tetherPrefab != null)
        {
            GameObject tetherObj = Instantiate(skill.tetherPrefab);
            tetherInstance = tetherObj.GetComponent<LineRenderer>();
        }

        // Ýleri atýl
        characterController.Move(transform.forward * skill.dashDistance);
        OnSpiritFormStateChanged?.Invoke(true); // UI'ý göster
        // UI ikonunu "Geri Dön" ikonuyla deðiþtir
        OnSkillChanged?.Invoke(2, skill.recastSkillIcon); // 2 = E slotu

        Debug.Log("Ruh Formu Aktif!");
    }

    void EndSpiritForm()
    {
        if (!isSpiritActive) return;

        // Bedeninin pozisyonuna geri dön
        // Iþýnlanma öncesi ve sonrasý CharacterController'ý kapatýp açmak en güvenli yoldur.
        characterController.enabled = false;
        transform.position = bodyPosition;
        characterController.enabled = true;

        // Beden kopyasýný ve efektleri yok et
        if (bodyInstance != null) Destroy(bodyInstance);
        if (spiritVFXInstance != null) Destroy(spiritVFXInstance);

        // Biriken hasarý hesapla
        float levelProgress = (playerStats.level - 1) / 99f;
        float percentToDeal = Mathf.Lerp(activeSpiritSkill.minDamageReturnPercent, activeSpiritSkill.maxDamageReturnPercent, levelProgress) / 100f;
        int finalDamage = Mathf.RoundToInt(totalDamageDealtInSpiritForm * percentToDeal);

        if (tetherInstance != null)
        {
            Destroy(tetherInstance.gameObject);
        }
        Debug.Log("Ruh Formu Bitti! Patlama Hasarý: " + finalDamage);

        // Hasar verdiðimiz tüm düþmanlara bu hasarý "Gerçek Hasar" olarak uygula
        foreach (CharacterStats enemy in damagedEnemies)
        {
            if (enemy != null) // Düþman o arada ölmüþ olabilir
            {
                enemy.TakeDamage(finalDamage, DamageType.True);
            }
        }

        // En sona, her þey bittikten sonra:
        // 1. Cooldown'u baþlat
        skillCooldowns[activeSpiritSkill] = playerStats.CalculateCooldown(activeSpiritSkill.cooldown);
        // 2. Ana skill bar'daki UI'ý güncelle
        OnSkillCooldownUpdate?.Invoke(2, skillCooldowns[activeSpiritSkill], activeSpiritSkill.cooldown);


        // Durumu sýfýrla ve UI'ý eski haline getir
        isSpiritActive = false;
        OnSpiritFormStateChanged?.Invoke(false); // UI'ý gizle
        OnSkillChanged?.Invoke(2, activeSpiritSkill); // 2 = E slotu




    }








    IEnumerator DelayedResetQCombo()
    {
        yield return new WaitForSeconds(0.5f);
        ResetQCombo();
    }
    private void ResetQCombo()
    {
        qComboCounter = 0;
        animator.SetInteger("Q_State", 0);
        IsQComboActive = false;
        if (currentQCombo != null)
        {
            OnQComboTimerUpdate?.Invoke(0, currentQCombo.comboResetTime);
        }
    }
    public void DoBuffSkill(BuffSkill skill)
    {
        animator.SetTrigger("Ultimate");
        isUltimateActive = true;
        ultimateEndTime = Time.time + skill.duration;
        activeBuffSkill = skill;
        playerStats.damage.AddModifier(skill.damageBonus);
        OnSkillChanged?.Invoke(3, skill.secondCastSkill);
    }
    private void DeactivateUltimate()
    {
        if (activeBuffSkill == null) return;
        playerStats.damage.RemoveModifier(activeBuffSkill.damageBonus);
        isUltimateActive = false;
        skillCooldowns[activeBuffSkill] = playerStats.CalculateCooldown(activeBuffSkill.cooldown);
        OnSkillChanged?.Invoke(3, activeBuffSkill);
        activeBuffSkill = null;
    }
    public void DoProjectileSkill(ProjectileSkill skill)
    {
        animator.SetTrigger("WindSlash");
        GameObject projectileObject = Instantiate(skill.projectilePrefab, transform.position + transform.forward + Vector3.up, transform.rotation);
        WindSlashProjectile wsp = projectileObject.GetComponent<WindSlashProjectile>();
        if (wsp != null)
        {
            wsp.damage = skill.damage;
        }
    }

    public void DoSummonSkill(SummonSkill skill)
    {
        animator.SetTrigger("Summon");
        Vector3 spawnPosition = transform.position + transform.forward * 2f;
        GameObject summon = Instantiate(skill.summonPrefab, spawnPosition, transform.rotation);

        HydraController hydra = summon.GetComponent<HydraController>();
        if (hydra != null)
        {
            // YENÝ: Hydra'nýn hasarýný hesapla ve ona bildir
            int hydraDamage = Mathf.RoundToInt(playerStats.abilityPower.GetValue() * skill.abilityPowerScaling);
            hydra.Initialize(hydraDamage);

            hydra.duration = skill.summonDuration;
        }

        Debug.Log(skill.name + " yeteneði kullanýldý, " + skill.summonPrefab.name + " çaðrýldý!");
    }

    public void DoHealSkill(Skill skill)
    {
        playerStats.Heal(skill.healAmount);
        if (skill.skillVFX != null && vfxSpawnPoint != null)
        {
            Instantiate(skill.skillVFX, vfxSpawnPoint.position, vfxSpawnPoint.rotation, vfxSpawnPoint);
        }
    }
    #endregion
}