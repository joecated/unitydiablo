// SpiritFormUI.cs (YEN� SCR�PT)
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpiritFormUI : MonoBehaviour
{
    public Image icon;
    public Image durationOverlay; // Cooldown yerine s�re
    public TextMeshProUGUI durationText;

    void Start()
    {
        if (PlayerCombat.instance != null) // PlayerCombat'e bir instance ekleyece�iz
        {
            PlayerCombat.instance.OnSpiritFormStateChanged += ToggleDisplay;
            PlayerCombat.instance.OnSpiritFormDurationUpdated += UpdateTimer;
        }
        gameObject.SetActive(false);
    }
    void OnDestroy() { /* ... event aboneli�inden ��k ... */ }

    void ToggleDisplay(bool show) { gameObject.SetActive(show); }

    void UpdateTimer(float current, float max)
    {
        durationOverlay.fillAmount = current / max;
        durationText.text = current.ToString("F1");
    }
}