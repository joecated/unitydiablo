// HealPopup.cs (YEN� SCR�PT)
// Bu script, olu�turulan iyile�tirme yaz�s�n�n davran���n� kontrol eder.
using UnityEngine;
using TMPro;

public class HealPopup : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private float disappearTimer;
    private Color textColor;

    // Olu�turuldu�unda �a�r�lacak kurulum fonksiyonu
    public void Setup(int healAmount)
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.SetText("+" + healAmount.ToString());
        textColor = textMesh.color;
        disappearTimer = 1f; // 1 saniye sonra kaybolacak
    }

    void Update()
    {
        // Yaz�y� yukar� do�ru hareket ettir
        float moveYSpeed = 1f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        // Zamanla yaz�y� soldur
        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                // Tamamen kaybolunca ebeveyni olan Canvas'� yok et
                Destroy(transform.parent.gameObject);
            }
        }
    }
}