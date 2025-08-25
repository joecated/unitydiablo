// HealPopupManager.cs (YENÝ SCRÝPT)
// Bu script, iyileþtirme yazýlarýnýn oluþturulmasýný yönetir.
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

        // Prefab'ý oluþtur
        GameObject popupInstance = Instantiate(healPopupPrefab, position + Vector3.up * 1.5f, Quaternion.identity);

        // Prefab'ýn içindeki yazý objesini bul ve script'ine eriþ
        HealPopup healPopup = popupInstance.GetComponentInChildren<HealPopup>();

        // Yazýyý ayarla
        if (healPopup != null)
        {
            healPopup.Setup(healAmount);
        }
    }
}