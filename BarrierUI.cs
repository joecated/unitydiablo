// BarrierUI.cs (TOOLTIP ÖZELLÝÐÝ EKLENDÝ)
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems; // Fare etkileþimleri için gerekli

public class BarrierUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;
    public Image cooldownOverlay;
    public TextMeshProUGUI cooldownText;

    private bool isBarrierCurrentlyReady = true; // Bariyerin anlýk durumunu tutmak için

    void Start()
    {
        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.OnBarrierStateChanged += UpdateState;
            PlayerStats.instance.OnBarrierCooldownUpdated += UpdateCooldown;
        }
        // Baþlangýçta ikonu tamamen gizle
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

    // YENÝ: Bu fonksiyon artýk sadece ikonu GÖSTERÝR.
    public void Show()
    {
        gameObject.SetActive(true);
    }

    // YENÝ: Bu fonksiyon artýk sadece ikonu GÝZLER.
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateState(bool isReady)
    {
        isBarrierCurrentlyReady = isReady; // Anlýk durumu güncelle
        
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

    // --- YENÝ EKLENEN TOOLTIP FONKSÝYONLARI ---

    public void OnPointerEnter(PointerEventData eventData)
    {
        string title = "Banshee'nin Duvaðý";
        string content;

        if (isBarrierCurrentlyReady)
        {
            content = "Bir sonraki hasarý engellemeye <color=green>Hazýr</color>.";
        }
        else
        {
            content = "Bariyer <color=red>Bekleme süresinde.</color>";
        }
        
        // Eþya tooltip'i için yaptýðýmýz TooltipManager'ý çaðýrýyoruz
        TooltipManager.instance.ShowTooltip(title, content);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Fare ayrýldýðýnda tooltip'i gizle
        TooltipManager.instance.HideTooltip();
    }
}