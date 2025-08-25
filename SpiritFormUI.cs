// SpiritFormUI.cs (YENÝ SCRÝPT)
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpiritFormUI : MonoBehaviour
{
    public Image icon;
    public Image durationOverlay; // Cooldown yerine süre
    public TextMeshProUGUI durationText;

    void Start()
    {
        if (PlayerCombat.instance != null) // PlayerCombat'e bir instance ekleyeceðiz
        {
            PlayerCombat.instance.OnSpiritFormStateChanged += ToggleDisplay;
            PlayerCombat.instance.OnSpiritFormDurationUpdated += UpdateTimer;
        }
        gameObject.SetActive(false);
    }
    void OnDestroy() { /* ... event aboneliðinden çýk ... */ }

    void ToggleDisplay(bool show) { gameObject.SetActive(show); }

    void UpdateTimer(float current, float max)
    {
        durationOverlay.fillAmount = current / max;
        durationText.text = current.ToString("F1");
    }
}