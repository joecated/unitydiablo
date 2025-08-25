// TooltipManager.cs
using UnityEngine;
using TMPro;
using System.Text; // StringBuilder için gerekli

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager instance;

    [Header("UI Referanslarý")]
    public GameObject tooltipPanel;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemStatsText;

    // Tooltip'in fare imlecinden ne kadar uzakta duracaðýný belirler
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

        // Baþlangýçta paneli gizle
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

    // Tooltip'i göstermek için çaðrýlacak fonksiyon
    public void ShowTooltip(Item item)
    {
        if (item == null) return;

        // Her tooltip'te eþyanýn ismini göster
        itemNameText.text = item.name;

        // Ýçerik metnini oluþturmak için bir metin birleþtirici kullanalým
        StringBuilder sb = new StringBuilder();

        // 1. ADIM: Eþyanýn ana açýklamasýný en üste ekle (varsa)
        if (!string.IsNullOrEmpty(item.description))
        {
            sb.AppendLine(item.description);
            sb.AppendLine(); // Bir satýr boþluk býrak
        }

        // 2. ADIM: Eþyanýn türüne göre özel bilgileri ekle
        if (item.itemType == ItemType.Equipment)
        {
            // Eðer eþya bir ekipmansa, onu Equipment türüne çevirip statlarýný yazdýr
            Equipment equipment = item as Equipment;
            if (equipment != null)
            {
                if (equipment.damageModifier > 0)
                    sb.AppendLine("<color=orange>+" + equipment.damageModifier + " Saldýrý Gücü</color>");
                if (equipment.armorModifier > 0)
                    sb.AppendLine("<color=yellow>+" + equipment.armorModifier + " Zýrh</color>");
                if (equipment.magicResistModifier > 0)
                    sb.AppendLine("<color=blue>+" + equipment.magicResistModifier + " Büyü Direnci</color>");
                if (equipment.abilityPowerModifier > 0)
                    sb.AppendLine("<color=#A335EE>+" + equipment.abilityPowerModifier + " Yetenek Gücü</color>");
                if (equipment.critChanceModifier > 0)
                    sb.AppendLine("<color=purple>+" + (equipment.critChanceModifier * 100f).ToString("F1") + "% Kritik Þansý</color>");
                if (equipment.critDamageModifier > 0)
                    sb.AppendLine("<color=red>+" + (equipment.critDamageModifier * 100f).ToString("F1") + "% Kritik Hasarý</color>");
                if (equipment.cooldownReductionModifier > 0)
                    sb.AppendLine("<color=blue>+" + (equipment.cooldownReductionModifier * 100f).ToString("F1") + "% Bekleme Süresi Az.</color>");
            }
        }
        else if (item.itemType == ItemType.Material)
        {
            // Materyaller için ekstra bir stat bilgisi göstermiyoruz.
            // Açýklamasý ve Kaynak bilgisi yeterli.
        }
        // Gelecekte 'Consumable' (Ýksir gibi) bir tür eklersen, buraya bir "else if" daha ekleyebilirsin.

        // 3. ADIM: Eþyanýn kaynak bilgisini en alta ekle (varsa)
        if (!string.IsNullOrEmpty(item.source))
        {
            sb.AppendLine(); // Bir satýr boþluk býrak
                             // Kaynak bilgisini daha soluk ve italik yazalým, daha þýk durur.
            sb.AppendLine("<color=#BDBDBD><i>Kaynak: " + item.source + "</i></color>");
        }

        // Oluþturulan metni UI'a ata
        itemStatsText.text = sb.ToString();

        // Paneli göster
        tooltipPanel.SetActive(true);
    }

    // YENÝ FONKSÝYON: Baþlýk ve içerik metni alarak tooltip gösterir.
    public void ShowTooltip(string title, string content)
    {
        // Metinleri ata
        itemNameText.text = title;
        itemStatsText.text = content;

        // Paneli göster
        tooltipPanel.SetActive(true);
    }

    // Tooltip'i gizlemek için çaðrýlacak fonksiyon
    public void HideTooltip()
    {
        if (tooltipPanel != null)
        {
            tooltipPanel.SetActive(false);
        }
    }
}