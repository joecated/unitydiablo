// EnemyHealthUI.cs (SAYISAL GÖSTERGE EKLENDÝ)
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyHealthUI : MonoBehaviour
{
    public GameObject healthBarPanel;
    public TextMeshProUGUI enemyNameText;
    public Slider healthSlider;
    public TextMeshProUGUI healthText; // YENÝ: Can sayýsýný gösterecek metin

    private CharacterStats currentTargetStats;

    void Start()
    {
        PlayerController pc = FindObjectOfType<PlayerController>();
        if (pc != null)
        {
            pc.OnNewTargetSelected += SetNewTarget;
        }
        healthBarPanel.SetActive(false);
    }

    void SetNewTarget(Transform newTarget)
    {
        if (currentTargetStats != null)
        {
            currentTargetStats.OnHealthChanged -= UpdateHealth;
        }

        if (newTarget != null)
        {
            currentTargetStats = newTarget.GetComponent<CharacterStats>();
            if (currentTargetStats != null)
            {
                currentTargetStats.OnHealthChanged += UpdateHealth;
                enemyNameText.text = newTarget.name.Replace("(Clone)", "");
                UpdateHealth(currentTargetStats.currentHealth, (int)currentTargetStats.maxHealth.GetValue());
                healthBarPanel.SetActive(true);
            }
        }
        else
        {
            healthBarPanel.SetActive(false);
        }
    }

    void UpdateHealth(int current, int max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = current;

        // YENÝ: Metni "100 / 100" formatýnda güncelle
        if (healthText != null)
        {
            healthText.text = current + " / " + max;
        }
    }
}