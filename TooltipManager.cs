// TooltipManager.cs
using UnityEngine;
using TMPro;
using System.Text; // StringBuilder i�in gerekli

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;

    [Header("UI Referanslar�")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemStatsText;

    // Tooltip'in fare imlecinden ne kadar uzakta duraca��n� belirler
    public Vector2 offset = new Vector2(15f, -15f);

    void Awake()
    {
        // Singleton Kurulumu
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Ba�lang��ta paneli gizle
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
        }
    }

    void Update()
    {
        // Panel aktifse fareyi takip et
        if (tooltipPanel.activeSelf)
        {
            tooltipPanel.transform.position = (Vector2)Input.mousePosition + offset;
        }
    }

    // Tooltip'i g�stermek i�in �a�r�lacak fonksiyon
    public void ShowTooltip(Item item)
    {
        if (item == null) return;

        // Her tooltip'te e�yan�n ismini g�ster
        itemNameText.text = item.name;

        // ��erik metnini olu�turmak i�in bir metin birle�tirici kullanal�m
        StringBuilder sb = new StringBuilder();

        // 1. ADIM: E�yan�n ana a��klamas�n� en �ste ekle (varsa)
        if (!string.IsNullOrEmpty(item.description))
        {
            sb.AppendLine(item.description);
            sb.AppendLine(); // Bir sat�r bo�luk b�rak
        }

        // 2. ADIM: E�yan�n t�r�ne g�re �zel bilgileri ekle
        if (item.itemType == ItemType.Equipment)
        {
            // E�er e�ya bir ekipmansa, onu Equipment t�r�ne �evirip statlar�n� yazd�r
            Equipment equipment = item as Equipment;
            if (equipment != null)
            {
                if (equipment.damageModifier > 0)
                    sb.AppendLine("<color=orange>+" + equipment.damageModifier + " Sald�r� G�c�</color>");
                if (equipment.armorModifier > 0)
                    sb.AppendLine("<color=yellow>+" + equipment.armorModifier + " Z�rh</color>");
                if (equipment.magicResistModifier > 0)
                    sb.AppendLine("<color=blue>+" + equipment.magicResistModifier + " B�y� Direnci</color>");
                if (equipment.abilityPowerModifier > 0)
                    sb.AppendLine("<color=#A335EE>+" + equipment.abilityPowerModifier + " Yetenek G�c�</color>");
                if (equipment.critChanceModifier > 0)
                    sb.AppendLine("<color=purple>+" + (equipment.critChanceModifier * 100f).ToString("F1") + "% Kritik �ans�</color>");
                if (equipment.critDamageModifier > 0)
                    sb.AppendLine("<color=red>+" + (equipment.critDamageModifier * 100f).ToString("F1") + "% Kritik Hasar�</color>");
                if (equipment.cooldownReductionModifier > 0)
                    sb.AppendLine("<color=blue>+" + (equipment.cooldownReductionModifier * 100f).ToString("F1") + "% Bekleme S�resi Az.</color>");
            }
        }
        else if (item.itemType == ItemType.Material)
        {
            // Materyaller i�in ekstra bir stat bilgisi g�stermiyoruz.
            // A��klamas� ve Kaynak bilgisi yeterli.
        }
        // Gelecekte 'Consumable' (�ksir gibi) bir t�r eklersen, buraya bir "else if" daha ekleyebilirsin.

        // 3. ADIM: E�yan�n kaynak bilgisini en alta ekle (varsa)
        if (!string.IsNullOrEmpty(item.source))
        {
            sb.AppendLine(); // Bir sat�r bo�luk b�rak
                             // Kaynak bilgisini daha soluk ve italik yazal�m, daha ��k durur.
            sb.AppendLine("<color=#BDBDBD><i>Kaynak: " + item.source + "</i></color>");
        }

        // Olu�turulan metni UI'a ata
        itemStatsText.text = sb.ToString();

        // Paneli g�ster
        tooltipPanel.SetActive(true);
    }

    // YEN� FONKS�YON: Ba�l�k ve i�erik metni alarak tooltip g�sterir.
    public void ShowTooltip(string title, string content)
    {
        // Metinleri ata
        itemNameText.text = title;
        itemStatsText.text = content;

        // Paneli g�ster
        tooltipPanel.SetActive(true);
    }

    // Tooltip'i gizlemek i�in �a�r�lacak fonksiyon
    public void HideTooltip()
    {
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
        }
    }
}