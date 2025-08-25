// DamagePopupManager.cs (YENÝDEN EKLENDÝ)
// Bu script, hasar yazýlarýnýn oluþturulmasýný yönetir.
using UnityEngine;

public class DamagePopupManager : MonoBehaviour
{
    public static DamagePopupManager instance;
    public GameObject damagePopupPrefab;

    void Awake()
    {
        instance = this;
    }

    public void CreatePopup(Vector3 position, int damageAmount)
    {
        if (damagePopupPrefab == null) return;
        GameObject popupInstance = Instantiate(damagePopupPrefab, position + Vector3.up * 1.5f, Quaternion.identity);
        DamagePopup damagePopup = popupInstance.GetComponentInChildren<DamagePopup>();
        if (damagePopup != null)
        {
            damagePopup.Setup(damageAmount);
        }
    }
}