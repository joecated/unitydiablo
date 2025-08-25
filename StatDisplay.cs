// StatDisplay.cs (G�NCELLEND�)
// Art�k XP bar�n�n �zerindeki yaz�y� da g�ncelleyecek.
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatDisplay : MonoBehaviour
{
    [Header("Can ve Mana")]
    public Slider healthSlider; // Orb yerine Slider
    public Slider manaSlider;   // Orb yerine Slider
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;

    [Header("Seviye ve XP")]
    public Slider xpSlider;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText; // XP yaz�s� i�in yeni referans

    private PlayerStats playerStats;

    void Start()
    {
        // PlayerStats'e art�k "instance" �zerinden g�venle eri�ebiliriz.
        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.OnHealthChanged += UpdateHealthUI;
            PlayerStats.instance.OnManaChanged += UpdateManaUI;
            PlayerStats.instance.OnXpChanged += UpdateXpUI;
            PlayerStats.instance.OnLevelChanged += UpdateLevelUI;

            // Ba�lang�� de�erlerini ata
            UpdateHealthUI(PlayerStats.instance.currentHealth, (int)PlayerStats.instance.maxHealth.GetValue());
            UpdateManaUI(PlayerStats.instance.currentMana, (int)PlayerStats.instance.maxMana.GetValue());
            UpdateXpUI(PlayerStats.instance.currentXp, PlayerStats.instance.xpToNextLevel);
            UpdateLevelUI(PlayerStats.instance.level);
        }
    }

    void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.OnHealthChanged -= UpdateHealthUI;
            playerStats.OnManaChanged -= UpdateManaUI;
            playerStats.OnXpChanged -= UpdateXpUI;
            playerStats.OnLevelChanged -= UpdateLevelUI;
        }
    }

    void UpdateHealthUI(int current, int max)
    {
        if (healthSlider != null) { healthSlider.maxValue = max; healthSlider.value = current; }
        if (healthText != null) healthText.text = current + " / " + max;
    }

    void UpdateManaUI(int current, int max)
    {
        if (manaSlider != null) { manaSlider.maxValue = max; manaSlider.value = current; }
        if (manaText != null) manaText.text = current + " / " + max;
    }
    void UpdateXpUI(int current, int max)
    {
        if (xpSlider != null)
        {
            xpSlider.maxValue = max;
            xpSlider.value = current;
        }

        // XP yaz�s�n� g�ncelle
        if (xpText != null)
        {
            xpText.text = current + " / " + max;
        }
    }

    void UpdateLevelUI(int level)
    {
        if (levelText != null) levelText.text = "Seviye: " + level;
    }
}