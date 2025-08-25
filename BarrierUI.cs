// BarrierUI.cs (TOOLTIP �ZELL��� EKLEND�)
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // Fare etkile�imleri i�in gerekli

public class BarrierUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public Image cooldownOverlay;
    public TextMeshProUGUI cooldownText;

    private bool isBarrierCurrentlyReady = true; // Bariyerin anl�k durumunu tutmak i�in

    void Start()
    {
        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.OnBarrierStateChanged += UpdateState;
            PlayerStats.instance.OnBarrierCooldownUpdated += UpdateCooldown;
        }
        // Ba�lang��ta ikonu tamamen gizle
        Hide();
    }

    void OnDestroy()
    {
        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.OnBarrierStateChanged -= UpdateState;
            PlayerStats.instance.OnBarrierCooldownUpdated -= UpdateCooldown;
        }
    }

    // YEN�: Bu fonksiyon art�k sadece ikonu G�STER�R.
    public void Show()
    {
        gameObject.SetActive(true);
    }

    // YEN�: Bu fonksiyon art�k sadece ikonu G�ZLER.
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateState(bool isReady)
    {
        isBarrierCurrentlyReady = isReady; // Anl�k durumu g�ncelle
        
        if (isReady)
        {
            icon.color = Color.white;
            cooldownOverlay.fillAmount = 0;
            cooldownText.text = "";
        }
        else
        {
            icon.color = Color.grey;
        }
    }

    void UpdateCooldown(float current, float max)
    {
        if (current > 0)
        {
            cooldownOverlay.fillAmount = current / max;
            cooldownText.text = Mathf.CeilToInt(current).ToString();
        }
    }

    // --- YEN� EKLENEN TOOLTIP FONKS�YONLARI ---

    public void OnPointerEnter(PointerEventData eventData)
    {
        string title = "Banshee'nin Duva��";
        string content;

        if (isBarrierCurrentlyReady)
        {
            content = "Bir sonraki hasar� engellemeye <color=green>Haz�r</color>.";
        }
        else
        {
            content = "Bariyer <color=red>Bekleme s�resinde.</color>";
        }
        
        // E�ya tooltip'i i�in yapt���m�z TooltipManager'� �a��r�yoruz
        TooltipManager.instance.ShowTooltip(title, content);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Fare ayr�ld���nda tooltip'i gizle
        TooltipManager.instance.HideTooltip();
    }
}