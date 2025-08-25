// SkillSlotUI.cs (T�M SORUNLARIN ��Z�LD��� F�NAL VERS�YON)
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
    private PlayerCombat playerCombat; // Referans� burada tutuyoruz

    void Awake()
    {
        tooltipTrigger = GetComponent<TooltipTrigger>();
        // Referanslar� oyunun en ba��nda, g�venli bir �ekilde al�yoruz.
        playerStats = FindObjectOfType<PlayerStats>();
        playerCombat = FindObjectOfType<PlayerCombat>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skill != null)
        {
            // G�NCELLEME: UpdateTooltip'i �a��rmak yerine, t�m mant��� buraya ta��yoruz.
            // Bu, olas� t�m zamanlama ve "hayalet metin" sorunlar�n� ortadan kald�r�r.

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
                    // METN� SIFIRDAN, ELLE OLU�TURUYORUZ. recastSkill.description'� OKUMUYORUZ.
                    content = "Bedenine an�nda geri d�n. Biriktirdi�in hasar�n <color=white>"
                              + percent.ToString("F0")
                              + "%</color> kadar�n� Ger�ek Hasar olarak patlat�rs�n.";
                }
                else
                {
                    // NORMAL HALDEYKEN
                    content = content.Replace("{duration}", spiritSkill.duration.ToString());
                    content = content.Replace("{%percent}", "<color=white>" + percent.ToString("F0") + "%</color>");
                }
            }
            else // Di�er t�m yetenekler i�in standart prosed�r
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

            // T�m bilgileri en son TooltipTrigger'a ata
            //tooltipTrigger.header = header;
            //tooltipTrigger.content = content;
            //tooltipTrigger.manaCost = skill.manaCost.ToString() + " Mana";
            //tooltipTrigger.cooldown = playerStats.CalculateCooldown(skill.cooldown).ToString("F1") + "s Cooldown";
            // Tooltip'i G�NCELLENM�� bilgilerle g�ster.
            TooltipSystem.Show(content, header, mana, cooldown);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }

    void UpdateTooltip()
    {
        // G�VENL�K KONTROL�: Awake'te ald���m�z referanslar� kullan�yoruz, .instance de�il.
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

                // RUH FORMUNDAYKEN ("Geri D�n" ikonu)
                Skill recastSkill = spiritSkill.recastSkillIcon;
                header = recastSkill.name;
                



                content = "Bedenine an�nda geri d�n. Biriktirdi�in hasar�n <color=white>"
                          + returnPercent.ToString("F0")
                          + "%</color> kadar�n� Ger�ek Hasar olarak patlat�rs�n.";
            }
            else
            {
                // NORMAL HALDEYKEN ("Ruh Bi�en" ikonu)
                header = spiritSkill.name;
                content = spiritSkill.description;

                float currentReturnPercent = playerCombat.GetCurrentSpiritDamagePercent(spiritSkill) * 100f;
                content = content.Replace("{duration}", spiritSkill.duration.ToString());
                content = content.Replace("{%percent}", "<color=white>" + currentReturnPercent.ToString("F0") + "%</color>");
            }
        }
        else // Di�er t�m yetenekler i�in standart prosed�r
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

        // T�m bilgileri en son TooltipTrigger'a ata
        tooltipTrigger.header = header;
        tooltipTrigger.content = content;
        tooltipTrigger.manaCost = skill.manaCost.ToString() + " Mana";
        tooltipTrigger.cooldown = playerStats.CalculateCooldown(skill.cooldown).ToString("F1") + "s Cooldown";
    }

    #region Eski Kodlar (Dokunulmad�)
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