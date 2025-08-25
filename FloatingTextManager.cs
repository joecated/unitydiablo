// FloatingTextManager.cs
using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager instance;
    public GameObject textPrefab;

    // YEN�: Bu de�eri de�i�tirerek yaz�n�n ne kadar yukar�da ��kaca��n� ayarlayabilirsin.
    public float verticalOffset = 50f;

    private Transform canvasTransform;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        canvasTransform = GetComponentInParent<Canvas>().transform;
    }

    public void ShowText(string text, Vector3 position, Color color)
    {
        if (textPrefab == null || canvasTransform == null) return;

        GameObject textObject = Instantiate(textPrefab, canvasTransform);

        // D�nya pozisyonunu ekran pozisyonuna �eviriyoruz.
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);

        // YEN�: Hesaplanan ekran pozisyonuna dikey bir ofset ekliyoruz.
        screenPosition.y += verticalOffset;

        textObject.transform.position = screenPosition;

        TextMeshProUGUI tmp = textObject.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.color = color;

        // BU SATIRI S�L�YORUZ, ��nk� art�k FloatingText.cs bu i�i yap�yor.
        // Destroy(textObject, 0.8f); 
    }
}