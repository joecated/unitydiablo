// XpPopupManager.cs (YEN� SCR�PT)
// Bu script, XP yaz�lar�n�n olu�turulmas�n� y�netir.
using UnityEngine;

public class XpPopupManager : MonoBehaviour
{
    public static XpPopupManager instance;

    public GameObject xpPopupPrefab; // Inspector'dan atanacak prefab

    void Awake()
    {
        instance = this;
    }

    public void CreatePopup(Vector3 position, int xpAmount)
    {
        if (xpPopupPrefab == null) return;

        // Prefab'� olu�tur
        GameObject popupInstance = Instantiate(xpPopupPrefab, position + Vector3.up * 2f, Quaternion.identity);

        // Prefab'�n i�indeki yaz� objesini bul ve script'ine eri�
        XpPopup xpPopup = popupInstance.GetComponentInChildren<XpPopup>();

        // Yaz�y� ayarla
        if (xpPopup != null)
        {
            xpPopup.Setup(xpAmount);
        }
    }
}