// SkillBarManager.cs (HATA DÜZELTÝLDÝ - Yeni Event'e Abone Olundu)
using UnityEngine;

public class SkillBarManager : MonoBehaviour
{
    public SkillSlotUI[] skillSlots;
    private PlayerCombat playerCombat;

    void Start()
    {
        playerCombat = FindFirstObjectByType<PlayerCombat>();
        if (playerCombat != null)
        {
            playerCombat.OnSkillCooldownUpdate += UpdateSkillCooldownUI;
            // SORUN 4 ÇÖZÜMÜ: Yeni event'e abone oluyoruz.
            playerCombat.OnSkillChanged += OnSkillChanged;
            InitializeUI();
        }
    }

    void OnDestroy()
    {
        if (playerCombat != null)
        {
            playerCombat.OnSkillCooldownUpdate -= UpdateSkillCooldownUI;
            // SORUN 4 ÇÖZÜMÜ: Abonelikten çýkýyoruz.
            playerCombat.OnSkillChanged -= OnSkillChanged;
        }
    }

    void InitializeUI()
    {
        for (int i = 0; i < skillSlots.Length; i++)
        {
            if (i < playerCombat.skills.Length && playerCombat.skills[i] != null)
            {
                skillSlots[i].SetSkill(playerCombat.skills[i]);
            }
        }
    }

    void UpdateSkillCooldownUI(int skillIndex, float remainingCooldown, float totalCooldown)
    {
        if (skillIndex < skillSlots.Length)
        {
            skillSlots[skillIndex].UpdateCooldown(remainingCooldown, totalCooldown);
        }
    }

    // SORUN 4 ÇÖZÜMÜ: Ulti ikonu deðiþtiðinde bu fonksiyon çalýþacak.
    void OnSkillChanged(int slotIndex, Skill newSkill)
    {
        if (slotIndex < skillSlots.Length)
        {
            skillSlots[slotIndex].SetSkill(newSkill);
        }
    }
}