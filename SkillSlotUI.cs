// SkillSlotUI.cs (TÜM SORUNLARIN ÇÖZÜLDÜÐÜ FÝNAL VERSÝYON)
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TooltipTrigger))]
public class SkillSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public Image cooldownOverlay;
    public TextMeshProUGUI keyText;
    public TextMeshProUGUI cooldownText;

    private Skill skill;
    private TooltipTrigger tooltipTrigger;
    private PlayerStats playerStats;
    private PlayerCombat playerCombat; // Referansý burada tutuyoruz

    void Awake()
    {
        tooltipTrigger = GetComponent<TooltipTrigger>();
        // Referanslarý oyunun en baþýnda, güvenli bir þekilde alýyoruz.
        playerStats = FindObjectOfType<PlayerStats>();
        playerCombat = FindObjectOfType<PlayerCombat>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skill != null)
        {
            // GÜNCELLEME: UpdateTooltip'i çaðýrmak yerine, tüm mantýðý buraya taþýyoruz.
            // Bu, olasý tüm zamanlama ve "hayalet metin" sorunlarýný ortadan kaldýrýr.

            if (tooltipTrigger == null || playerStats == null || playerCombat == null) return;

            string header = skill.name;
            string content = skill.description;
            string mana = skill.manaCost.ToString() + " Mana";
            string cooldown = playerStats.CalculateCooldown(skill.cooldown).ToString("F1") + "s Cooldown";

            if (skill is SpiritSkill spiritSkill)
            {
                float percent = playerCombat.GetCurrentSpiritDamagePercent(spiritSkill) * 100f;

                if (playerCombat.IsSpiritActive())
                {
                    // RUH FORMUNDAYKEN
                    Skill recastSkill = spiritSkill.recastSkillIcon;
                    header = recastSkill.name;
                    // METNÝ SIFIRDAN, ELLE OLUÞTURUYORUZ. recastSkill.description'ý OKUMUYORUZ.
                    content = "Bedenine anýnda geri dön. Biriktirdiðin hasarýn <color=white>"
                              + percent.ToString("F0")
                              + "%</color> kadarýný Gerçek Hasar olarak patlatýrsýn.";
                }
                else
                {
                    // NORMAL HALDEYKEN
                    content = content.Replace("{duration}", spiritSkill.duration.ToString());
                    content = content.Replace("{%percent}", "<color=white>" + percent.ToString("F0") + "%</color>");
                }
            }
            else // Diðer tüm yetenekler için standart prosedür
            {
                header = skill.name;
                content = skill.description;

                if (skill is SummonSkill summonSkill)
                {
                    int damageValue = Mathf.RoundToInt(playerStats.abilityPower.GetValue() * summonSkill.abilityPowerScaling);
                    content = content.Replace("{damage}", "<color=orange>" + damageValue + "</color>");
                    content = content.Replace("{duration}", summonSkill.summonDuration.ToString());

                }
                else if (skill is BuffSkill buffSkill)
                {
                    if (content.Contains("{damageBonus}"))
                    {
                        content = content.Replace("{damageBonus}", "<color=yellow>" + buffSkill.damageBonus + "</color>");
                    }
                }
                else
                {
                    if (content.Contains("{damage}"))
                    {
                        int damageValue = Mathf.RoundToInt(playerStats.damage.GetValue()) + skill.damage;
                        content = content.Replace("{damage}", "<color=orange>" + damageValue + "</color>");
                    }
                }

                if (content.Contains("{heal}"))
                {
                    content = content.Replace("{heal}", "<color=green>" + skill.healAmount + "</color>");
                }
            }

            // Tüm bilgileri en son TooltipTrigger'a ata
            //tooltipTrigger.header = header;
            //tooltipTrigger.content = content;
            //tooltipTrigger.manaCost = skill.manaCost.ToString() + " Mana";
            //tooltipTrigger.cooldown = playerStats.CalculateCooldown(skill.cooldown).ToString("F1") + "s Cooldown";
            // Tooltip'i GÜNCELLENMÝÞ bilgilerle göster.
            TooltipSystem.Show(content, header, mana, cooldown);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }

    void UpdateTooltip()
    {
        // GÜVENLÝK KONTROLÜ: Awake'te aldýðýmýz referanslarý kullanýyoruz, .instance deðil.
        if (tooltipTrigger == null || skill == null || playerStats == null || playerCombat == null)
        {
            return;
        }

        string header;
        string content;

        if (skill is SpiritSkill spiritSkill)

        {

            float returnPercent = playerCombat.GetCurrentSpiritDamagePercent(spiritSkill) * 100f;

            if (playerCombat.IsSpiritActive())
            {

                // RUH FORMUNDAYKEN ("Geri Dön" ikonu)
                Skill recastSkill = spiritSkill.recastSkillIcon;
                header = recastSkill.name;
                



                content = "Bedenine anýnda geri dön. Biriktirdiðin hasarýn <color=white>"
                          + returnPercent.ToString("F0")
                          + "%</color> kadarýný Gerçek Hasar olarak patlatýrsýn.";
            }
            else
            {
                // NORMAL HALDEYKEN ("Ruh Biçen" ikonu)
                header = spiritSkill.name;
                content = spiritSkill.description;

                float currentReturnPercent = playerCombat.GetCurrentSpiritDamagePercent(spiritSkill) * 100f;
                content = content.Replace("{duration}", spiritSkill.duration.ToString());
                content = content.Replace("{%percent}", "<color=white>" + currentReturnPercent.ToString("F0") + "%</color>");
            }
        }
        else // Diðer tüm yetenekler için standart prosedür
        {
            header = skill.name;
            content = skill.description;

            if (skill is SummonSkill summonSkill)
            {
                int damageValue = Mathf.RoundToInt(playerStats.abilityPower.GetValue() * summonSkill.abilityPowerScaling);
                content = content.Replace("{damage}", "<color=orange>" + damageValue + "</color>");
                content = content.Replace("{duration}", summonSkill.summonDuration.ToString());

            }
            else if (skill is BuffSkill buffSkill)
            {
                if (content.Contains("{damageBonus}"))
                {
                    content = content.Replace("{damageBonus}", "<color=yellow>" + buffSkill.damageBonus + "</color>");
                }
            }
            else
            {
                if (content.Contains("{damage}"))
                {
                    int damageValue = Mathf.RoundToInt(playerStats.damage.GetValue()) + skill.damage;
                    content = content.Replace("{damage}", "<color=orange>" + damageValue + "</color>");
                }
            }

            if (content.Contains("{heal}"))
            {
                content = content.Replace("{heal}", "<color=green>" + skill.healAmount + "</color>");
            }
        }

        // Tüm bilgileri en son TooltipTrigger'a ata
        tooltipTrigger.header = header;
        tooltipTrigger.content = content;
        tooltipTrigger.manaCost = skill.manaCost.ToString() + " Mana";
        tooltipTrigger.cooldown = playerStats.CalculateCooldown(skill.cooldown).ToString("F1") + "s Cooldown";
    }

    #region Eski Kodlar (Dokunulmadý)
    public void SetSkill(Skill newSkill)
    {
        skill = newSkill;
        if (icon != null)
        {
            if (skill != null && skill.icon != null)
            {
                icon.sprite = skill.icon;
                icon.enabled = true;
            }
            else
            {
                icon.enabled = false;
            }
        }
        if (cooldownText != null)
        {
            cooldownText.gameObject.SetActive(false);
        }
    }
    public void UpdateCooldown(float remainingCooldown, float totalCooldown)
    {
        if (cooldownOverlay == null) return;
        if (remainingCooldown > 0)
        {
            if (!cooldownOverlay.enabled) cooldownOverlay.enabled = true;
            cooldownOverlay.fillAmount = remainingCooldown / totalCooldown;
            if (cooldownText != null)
            {
                if (!cooldownText.gameObject.activeSelf) cooldownText.gameObject.SetActive(true);
                int currentSecond = Mathf.CeilToInt(remainingCooldown);
                if (currentSecond != lastCooldownSecond)
                {
                    cooldownText.text = currentSecond.ToString();
                    lastCooldownSecond = currentSecond;
                }
            }
        }
        else
        {
            if (cooldownOverlay.enabled) cooldownOverlay.enabled = false;
            if (cooldownText != null && cooldownText.gameObject.activeSelf)
            {
                cooldownText.gameObject.SetActive(false);
            }
            lastCooldownSecond = -1;
        }
    }
    private int lastCooldownSecond = -1;
    #endregion
}