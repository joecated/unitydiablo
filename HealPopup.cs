// HealPopup.cs (YENÝ SCRÝPT)
// Bu script, oluþturulan iyileþtirme yazýsýnýn davranýþýný kontrol eder.
using UnityEngine;
using TMPro;

public class HealPopup : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private float disappearTimer;
    private Color textColor;

    // Oluþturulduðunda çaðrýlacak kurulum fonksiyonu
    public void Setup(int healAmount)
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.SetText("+" + healAmount.ToString());
        textColor = textMesh.color;
        disappearTimer = 1f; // 1 saniye sonra kaybolacak
    }

    void Update()
    {
        // Yazýyý yukarý doðru hareket ettir
        float moveYSpeed = 1f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        // Zamanla yazýyý soldur
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                // Tamamen kaybolunca ebeveyni olan Canvas'ý yok et
                Destroy(transform.parent.gameObject);
            }
        }
    }
}