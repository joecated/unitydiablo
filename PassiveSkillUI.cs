// PassiveSkillUI.cs
using UnityEngine;
using UnityEngine.UI;

public class PassiveSkillUI : MonoBehaviour
{
    public Image passiveIcon;
    public Color passiveReadyColor = Color.white; // Pasif yüklendiðinde ikonun alacaðý renk
    public Color passiveUsedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Kullanýldýðýnda soluklaþsýn

    private PlayerCombat playerCombat;

    void Start()
    {
        // PlayerCombat script'ini bul ve event'ine abone ol
        playerCombat = PlayerStats.instance.GetComponent<PlayerCombat>();
        if (playerCombat != null)
        {
            playerCombat.OnPassiveStateChanged += UpdateIcon;
        }

        // Baþlangýç durumunu ayarla (pasif yüklü deðil)
        UpdateIcon(false);
    }

    void OnDestroy()
    {
        // Abonelikten çýkmayý unutma
        if (playerCombat != null)
        {
            playerCombat.OnPassiveStateChanged -= UpdateIcon;
        }
    }

    // Bu fonksiyon, PlayerCombat'tan sinyal geldiðinde çalýþacak
    void UpdateIcon(bool isCharged)
    {
        if (passiveIcon != null)
        {
            // Gelen duruma göre ikonun rengini deðiþtir
            passiveIcon.color = isCharged ? passiveReadyColor : passiveUsedColor;
        }
    }
}