// PassiveSkillTooltipTrigger.cs (YETENEK SÝSTEMÝNE UYUMLU HALÝ)
using UnityEngine;
using UnityEngine.EventSystems;

public class PassiveSkillTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Tooltip Ýçeriði")]
    public string title = "Rünlü Kýlýç";
    [TextArea]
    public string description = "Bir yetenek kullandýktan sonraki ilk normal saldýrýn, seviyene ve saldýrý gücüne baðlý olarak bonus fiziksel hasar verir.";

    private PlayerCombat playerCombat;

    void Start()
    {
        // PlayerCombat'e referansý alýyoruz ki hasarý sorabilelim
        playerCombat = PlayerStats.instance.GetComponent<PlayerCombat>();
    }

    // Fare ikonun üzerine geldiðinde...
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (playerCombat == null) return;

        // PlayerCombat'e o anki bonus hasarýn ne olacaðýný soruyoruz
        int bonusDamage = playerCombat.GetCurrentPassiveBonusDamage();

        // Açýklama ve bonus hasarý birleþtiriyoruz
        string content = description + "\n\n<color=#FDB14AFF>Mevcut Bonus Hasar: " + bonusDamage + "</color>";

        // Senin mevcut TooltipSystem'ini kullanarak paneli gösteriyoruz.
        // Pasifin manasý ve bekleme süresi olmadýðý için o alanlara boþ metin ("") gönderiyoruz.
        TooltipSystem.Show(content, title, "", "");
    }

    // Fare ikonun üzerinden ayrýldýðýnda...
    public void OnPointerExit(PointerEventData eventData)
    {
        // Senin mevcut TooltipSystem'ini kullanarak paneli gizliyoruz.
        TooltipSystem.Hide();
    }
}