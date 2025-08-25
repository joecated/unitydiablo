// CharacterPanelUI.cs (ANINDA GÜNCELLEME ÝÇÝN DÜZELTÝLDÝ)
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CharacterPanelUI : MonoBehaviour
{
    public GameObject characterPanel;
    public Transform equipmentSlotsParent;

    public TextMeshProUGUI damageText;
    public TextMeshProUGUI armorText;
    // --- BURAYA YENÝ SATIRLARI EKLE ---
    public TextMeshProUGUI magicResistText;
    public TextMeshProUGUI abilityPowerText;
    public TextMeshProUGUI critChanceText;
    public TextMeshProUGUI critDamageText;
    public TextMeshProUGUI cooldownReductionText;
    // ---------------------------------

    EquipmentSlotUI[] equipmentSlots;
    EquipmentManager equipmentManager;
    private PlayerStats playerStats; // Sadece tek bir playerStats deðiþkenimiz var.


    void Start()
    {
        // 1. ADIM: playerStats deðiþkenini oyun baþlar baþlamaz dolduruyoruz.
        playerStats = PlayerStats.instance;
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats.instance bulunamadý!");
            return;
        }

        // Gerekli event'lere abone oluyoruz.
        if (EquipmentManager.instance != null)
        {
            EquipmentManager.instance.onEquipmentChanged += OnEquipmentChanged;
        }
        playerStats.OnStatsChanged += UpdateStatsUI;

        equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlotUI>();
        characterPanel.SetActive(false);
    }

    void OnDestroy()
    {
        // Ekipman yöneticisinden doðru fonksiyonla abonelikten çýk.
        if (EquipmentManager.instance != null)
        {
            EquipmentManager.instance.onEquipmentChanged -= OnEquipmentChanged;
        }

        // DÜZELTME: PlayerStats'tan da doðru fonksiyonla abonelikten çýk.
        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.OnStatsChanged -= UpdateStatsUI;
        }
    }

    // Bu fonksiyon, EquipmentManager'dan sinyal geldiðinde çalýþacak.
    // Görevi, doðru slotu bulup ikonunu güncellemek.
    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        // Bir eþya çýkarýldýysa, onun slotunu temizle.
        if (oldItem != null)
        {
            equipmentSlots[(int)oldItem.equipSlot].ClearSlot();
        }

        // Yeni bir eþya takýldýysa, onun slotuna ikonunu ekle.
        if (newItem != null)
        {
            equipmentSlots[(int)newItem.equipSlot].AddItem(newItem);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            characterPanel.SetActive(!characterPanel.activeSelf);
            if (characterPanel.activeSelf)
            {
                UpdateStatsUI(); // Paneli açtýðýmýzda da güncelleyelim.
            }
        }
    }

    // Bu fonksiyon artýk sadece ekipman ikonlarýný güncelliyor
    void UpdateEquipmentSlots(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            equipmentSlots[(int)newItem.equipSlot].AddItem(newItem);
        }
        else if (oldItem != null) // Bir eþya çýkarýldýysa
        {
            equipmentSlots[(int)oldItem.equipSlot].ClearSlot();
        }
    }

    // Bu fonksiyon, PlayerStats'tan sinyal geldiðinde çalýþacak.
    // Görevi, sadece stat yazýlarýný güncellemek.
    void UpdateStatsUI()
    {
        if (playerStats != null)
        {
            damageText.text = "Saldýrý Gücü: " + playerStats.damage.GetValue().ToString();
            armorText.text = "Zýrh: " + playerStats.armor.GetValue().ToString();
            magicResistText.text = "Büyü Direnci: " + playerStats.magicResist.GetValue().ToString();
            abilityPowerText.text = "Yetenek Gücü: " + playerStats.abilityPower.GetValue().ToString();

            float critChancePercent = playerStats.critChance.GetValue() * 100f;
            critChanceText.text = "Kritik Vuruþ Þansý: " + critChancePercent.ToString("F1") + "%";

            float critDamagePercent = playerStats.critDamage.GetValue() * 100f;
            critDamageText.text = "Kritik Vuruþ Hasarý: +" + critDamagePercent.ToString("F1") + "%";

            float cdrPercent = playerStats.cooldownReduction.GetValue() * 1f;
            cooldownReductionText.text = "Bekleme Süresi Az.: " + cdrPercent.ToString("F1") + "%";
        }
    }
}