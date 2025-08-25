// DamagePopup.cs (YENÝDEN EKLENDÝ)
// Bu script, oluþturulan hasar yazýsýnýn davranýþýný kontrol eder.
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private float disappearTimer;
    private Color textColor;

    public void Setup(int damageAmount)
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.SetText(damageAmount.ToString());
        textColor = textMesh.color;
        disappearTimer = 1f;
    }

    void Update()
    {
        float moveYSpeed = 2f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(transform.parent.gameObject);
            }
        }
    }
}