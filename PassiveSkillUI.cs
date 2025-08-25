// PassiveSkillUI.cs
using UnityEngine;
using UnityEngine.UI;

public class PassiveSkillUI : MonoBehaviour
{
    public Image passiveIcon;
    public Color passiveReadyColor = Color.white; // Pasif y�klendi�inde ikonun alaca�� renk
    public Color passiveUsedColor = new Color(0.5f, 0.5f, 0.5f, 0.5f); // Kullan�ld���nda solukla�s�n

    private PlayerCombat playerCombat;

    void Start()
    {
        // PlayerCombat script'ini bul ve event'ine abone ol
        playerCombat = PlayerStats.instance.GetComponent<PlayerCombat>();
        if (playerCombat != null)
        {
            playerCombat.OnPassiveStateChanged += UpdateIcon;
        }

        // Ba�lang�� durumunu ayarla (pasif y�kl� de�il)
        UpdateIcon(false);
    }

    void OnDestroy()
    {
        // Abonelikten ��kmay� unutma
        if (playerCombat != null)
        {
            playerCombat.OnPassiveStateChanged -= UpdateIcon;
        }
    }

    // Bu fonksiyon, PlayerCombat'tan sinyal geldi�inde �al��acak
    void UpdateIcon(bool isCharged)
    {
        if (passiveIcon != null)
        {
            // Gelen duruma g�re ikonun rengini de�i�tir
            passiveIcon.color = isCharged ? passiveReadyColor : passiveUsedColor;
        }
    }
}