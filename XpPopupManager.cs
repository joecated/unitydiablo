// XpPopupManager.cs (YENÝ SCRÝPT)
// Bu script, XP yazýlarýnýn oluþturulmasýný yönetir.
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

        // Prefab'ý oluþtur
        GameObject popupInstance = Instantiate(xpPopupPrefab, position + Vector3.up * 2f, Quaternion.identity);

        // Prefab'ýn içindeki yazý objesini bul ve script'ine eriþ
        XpPopup xpPopup = popupInstance.GetComponentInChildren<XpPopup>();

        // Yazýyý ayarla
        if (xpPopup != null)
        {
            xpPopup.Setup(xpAmount);
        }
    }
}