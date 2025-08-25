// PassiveSkillTooltipTrigger.cs (YETENEK S�STEM�NE UYUMLU HAL�)
using UnityEngine;
using UnityEngine.EventSystems;

public class PassiveSkillTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tooltip ��eri�i")]
    public string title = "R�nl� K�l��";
    [TextArea]
    public string description = "Bir yetenek kulland�ktan sonraki ilk normal sald�r�n, seviyene ve sald�r� g�c�ne ba�l� olarak bonus fiziksel hasar verir.";

    private PlayerCombat playerCombat;

    void Start()
    {
        // PlayerCombat'e referans� al�yoruz ki hasar� sorabilelim
        playerCombat = PlayerStats.instance.GetComponent<PlayerCombat>();
    }

    // Fare ikonun �zerine geldi�inde...
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (playerCombat == null) return;

        // PlayerCombat'e o anki bonus hasar�n ne olaca��n� soruyoruz
        int bonusDamage = playerCombat.GetCurrentPassiveBonusDamage();

        // A��klama ve bonus hasar� birle�tiriyoruz
        string content = description + "\n\n<color=#FDB14AFF>Mevcut Bonus Hasar: " + bonusDamage + "</color>";

        // Senin mevcut TooltipSystem'ini kullanarak paneli g�steriyoruz.
        // Pasifin manas� ve bekleme s�resi olmad��� i�in o alanlara bo� metin ("") g�nderiyoruz.
        TooltipSystem.Show(content, title, "", "");
    }

    // Fare ikonun �zerinden ayr�ld���nda...
    public void OnPointerExit(PointerEventData eventData)
    {
        // Senin mevcut TooltipSystem'ini kullanarak paneli gizliyoruz.
        TooltipSystem.Hide();
    }
}