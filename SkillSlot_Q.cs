using UnityEngine;
using UnityEngine.UI;

public class SkillSlot_Q : MonoBehaviour
{
    [Header("UI Referanslarý")]
    [SerializeField] private Image comboFrameImage; // Zamanlayýcýyý gösterecek çerçeve

    [Header("Player Referansý")]
    [SerializeField] private PlayerCombat playerCombat;

    void Start()
    {
        if (comboFrameImage == null)
        {
            Debug.LogError("Combo Frame Image referansý atanmamýþ!", this);
            return;
        }

        // Baþlangýçta çerçeveyi gizle
        comboFrameImage.gameObject.SetActive(false);

        // PlayerCombat event'ine abone ol
        if (playerCombat != null)
        {
            playerCombat.OnQComboTimerUpdate += UpdateComboFrame;
        }
        else
        {
            Debug.LogError("PlayerCombat referansý bu scripte atanmamýþ!", this);
        }
    }

    private void OnDestroy()
    {
        // Event aboneliðini iptal et
        if (playerCombat != null)
        {
            playerCombat.OnQComboTimerUpdate -= UpdateComboFrame;
        }
    }

    /// <summary>
    /// Kombo zamanlayýcýsý güncellendiðinde bu fonksiyon çalýþýr.
    /// </summary>
    private void UpdateComboFrame(float remainingTime, float totalTime)
    {
        // Eðer kombo için hala süre varsa...
        if (remainingTime > 0)
        {
            // Çerçeveyi görünür yap
            comboFrameImage.gameObject.SetActive(true);

            // Çerçevenin doluluk oranýný (fillAmount) kalan süreye göre ayarla.
            // Bu, çerçevenin zamanla dairesel olarak kaybolmasýný saðlar.
            comboFrameImage.fillAmount = remainingTime / totalTime;
        }
        // Eðer süre dolduysa...
        else
        {
            // Çerçeveyi gizle
            comboFrameImage.gameObject.SetActive(false);
        }
    }
}
