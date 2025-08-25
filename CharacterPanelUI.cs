// CharacterPanelUI.cs (ANINDA G�NCELLEME ���N D�ZELT�LD�)
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CharacterPanelUI : MonoBehaviour
{
    public GameObject characterPanel;
    public Transform equipmentSlotsParent;

    public TextMeshProUGUI damageText;
    public TextMeshProUGUI armorText;
    // --- BURAYA YEN� SATIRLARI EKLE ---
    public TextMeshProUGUI magicResistText;
    public TextMeshProUGUI abilityPowerText;
    public TextMeshProUGUI critChanceText;
    public TextMeshProUGUI critDamageText;
    public TextMeshProUGUI cooldownReductionText;
    // ---------------------------------

    EquipmentSlotUI[] equipmentSlots;
    EquipmentManager equipmentManager;
    private PlayerStats playerStats; // Sadece tek bir playerStats de�i�kenimiz var.


    void Start()
    {
        // 1. ADIM: playerStats de�i�kenini oyun ba�lar ba�lamaz dolduruyoruz.
        playerStats = PlayerStats.instance;
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats.instance bulunamad�!");
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
        // Ekipman y�neticisinden do�ru fonksiyonla abonelikten ��k.
        if (EquipmentManager.instance != null)
        {
            EquipmentManager.instance.onEquipmentChanged -= OnEquipmentChanged;
        }

        // D�ZELTME: PlayerStats'tan da do�ru fonksiyonla abonelikten ��k.
        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.OnStatsChanged -= UpdateStatsUI;
        }
    }

    // Bu fonksiyon, EquipmentManager'dan sinyal geldi�inde �al��acak.
    // G�revi, do�ru slotu bulup ikonunu g�ncellemek.
    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {
        // Bir e�ya ��kar�ld�ysa, onun slotunu temizle.
        if (oldItem != null)
        {
            equipmentSlots[(int)oldItem.equipSlot].ClearSlot();
        }

        // Yeni bir e�ya tak�ld�ysa, onun slotuna ikonunu ekle.
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
                UpdateStatsUI(); // Paneli a�t���m�zda da g�ncelleyelim.
            }
        }
    }

    // Bu fonksiyon art�k sadece ekipman ikonlar�n� g�ncelliyor
    void UpdateEquipmentSlots(Equipment newItem, Equipment oldItem)
    {
        if (newItem != null)
        {
            equipmentSlots[(int)newItem.equipSlot].AddItem(newItem);
        }
        else if (oldItem != null) // Bir e�ya ��kar�ld�ysa
        {
            equipmentSlots[(int)oldItem.equipSlot].ClearSlot();
        }
    }

    // Bu fonksiyon, PlayerStats'tan sinyal geldi�inde �al��acak.
    // G�revi, sadece stat yaz�lar�n� g�ncellemek.
    void UpdateStatsUI()
    {
        if (playerStats != null)
        {
            damageText.text = "Sald�r� G�c�: " + playerStats.damage.GetValue().ToString();
            armorText.text = "Z�rh: " + playerStats.armor.GetValue().ToString();
            magicResistText.text = "B�y� Direnci: " + playerStats.magicResist.GetValue().ToString();
            abilityPowerText.text = "Yetenek G�c�: " + playerStats.abilityPower.GetValue().ToString();

            float critChancePercent = playerStats.critChance.GetValue() * 100f;
            critChanceText.text = "Kritik Vuru� �ans�: " + critChancePercent.ToString("F1") + "%";

            float critDamagePercent = playerStats.critDamage.GetValue() * 100f;
            critDamageText.text = "Kritik Vuru� Hasar�: +" + critDamagePercent.ToString("F1") + "%";

            float cdrPercent = playerStats.cooldownReduction.GetValue() * 1f;
            cooldownReductionText.text = "Bekleme S�resi Az.: " + cdrPercent.ToString("F1") + "%";
        }
    }
}