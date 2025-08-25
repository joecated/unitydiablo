using UnityEngine;
using UnityEngine.UI;

public class SkillSlot_Q : MonoBehaviour
{
    [Header("UI Referanslar�")]
    [SerializeField] private Image comboFrameImage; // Zamanlay�c�y� g�sterecek �er�eve

    [Header("Player Referans�")]
    [SerializeField] private PlayerCombat playerCombat;

    void Start()
    {
        if (comboFrameImage == null)
        {
            Debug.LogError("Combo Frame Image referans� atanmam��!", this);
            return;
        }

        // Ba�lang��ta �er�eveyi gizle
        comboFrameImage.gameObject.SetActive(false);

        // PlayerCombat event'ine abone ol
        if (playerCombat != null)
        {
            playerCombat.OnQComboTimerUpdate += UpdateComboFrame;
        }
        else
        {
            Debug.LogError("PlayerCombat referans� bu scripte atanmam��!", this);
        }
    }

    private void OnDestroy()
    {
        // Event aboneli�ini iptal et
        if (playerCombat != null)
        {
            playerCombat.OnQComboTimerUpdate -= UpdateComboFrame;
        }
    }

    /// <summary>
    /// Kombo zamanlay�c�s� g�ncellendi�inde bu fonksiyon �al���r.
    /// </summary>
    private void UpdateComboFrame(float remainingTime, float totalTime)
    {
        // E�er kombo i�in hala s�re varsa...
        if (remainingTime > 0)
        {
            // �er�eveyi g�r�n�r yap
            comboFrameImage.gameObject.SetActive(true);

            // �er�evenin doluluk oran�n� (fillAmount) kalan s�reye g�re ayarla.
            // Bu, �er�evenin zamanla dairesel olarak kaybolmas�n� sa�lar.
            comboFrameImage.fillAmount = remainingTime / totalTime;
        }
        // E�er s�re dolduysa...
        else
        {
            // �er�eveyi gizle
            comboFrameImage.gameObject.SetActive(false);
        }
    }
}
