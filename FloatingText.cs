// FloatingText.cs (YEN� SCRIPT)
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float moveSpeed = 50f;     // Yaz�n�n yukar� kayma h�z�
    public float fadeOutTime = 1f;    // Yaz�n�n ne kadar s�rede kaybolaca��

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
        // Yaz�y� her saniye yukar� do�ru hareket ettir
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);

        // Zamanla yaz�n�n rengini silikle�tir (alpha de�erini d���r)
        timeElapsed += Time.deltaTime;
        float fadeAlpha = 1f - (timeElapsed / fadeOutTime);
        textMesh.color = new Color(startColor.r, startColor.g, startColor.b, fadeAlpha);

        // S�resi doldu�unda objeyi yok et
        if (timeElapsed > fadeOutTime)
        {
            Destroy(gameObject);
        }
    }
}