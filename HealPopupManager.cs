// HealPopupManager.cs (YEN� SCR�PT)
// Bu script, iyile�tirme yaz�lar�n�n olu�turulmas�n� y�netir.
using UnityEngine;

public class HealPopupManager : MonoBehaviour
{
    public static HealPopupManager instance;

    public GameObject healPopupPrefab; // Inspector'dan atanacak prefab

    void Awake()
    {
        instance = this;
    }

    public void CreatePopup(Vector3 position, int healAmount)
    {
        if (healPopupPrefab == null) return;

        // Prefab'� olu�tur
        GameObject popupInstance = Instantiate(healPopupPrefab, position + Vector3.up * 1.5f, Quaternion.identity);

        // Prefab'�n i�indeki yaz� objesini bul ve script'ine eri�
        HealPopup healPopup = popupInstance.GetComponentInChildren<HealPopup>();

        // Yaz�y� ayarla
        if (healPopup != null)
        {
            healPopup.Setup(healAmount);
        }
    }
}