using UnityEngine;
using UnityEngine.UI;

public class PlayerOverheadUI : MonoBehaviour
{
    public Slider healthSlider;
    public Slider manaSlider;
    private Transform mainCamera;

    void Start()
    {
        mainCamera = Camera.main.transform;
        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.OnHealthChanged += UpdateHealth;
            PlayerStats.instance.OnManaChanged += UpdateMana;
            // Baþlangýç deðerlerini ata
            UpdateHealth(PlayerStats.instance.currentHealth, (int)PlayerStats.instance.maxHealth.GetValue());
            UpdateMana(PlayerStats.instance.currentMana, (int)PlayerStats.instance.maxMana.GetValue());
        }
    }

    void LateUpdate()
    {
        // Canvas'ýn her zaman kameraya bakmasýný saðla
        transform.LookAt(transform.position + mainCamera.forward);
    }

    void UpdateHealth(int current, int max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;
    }
    void UpdateMana(int current, int max)
    {
        manaSlider.maxValue = max;
        manaSlider.value = current;
    }
}