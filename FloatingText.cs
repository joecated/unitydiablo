// FloatingText.cs (YENÝ SCRIPT)
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 50f;     // Yazýnýn yukarý kayma hýzý
    public float fadeOutTime = 1f;    // Yazýnýn ne kadar sürede kaybolacaðý

    private TextMeshProUGUI textMesh;
    private float timeElapsed = 0f;
    private Color startColor;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        startColor = textMesh.color;
    }

    void Update()
    {
        // Yazýyý her saniye yukarý doðru hareket ettir
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // Zamanla yazýnýn rengini silikleþtir (alpha deðerini düþür)
        timeElapsed += Time.deltaTime;
        float fadeAlpha = 1f - (timeElapsed / fadeOutTime);
        textMesh.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);

        // Süresi dolduðunda objeyi yok et
        if (timeElapsed > fadeOutTime)
        {
            Destroy(gameObject);
        }
    }
}