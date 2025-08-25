// FloatingTextManager.cs
using UnityEngine;
using TMPro;

public class FloatingTextManager : MonoBehaviour
{
    public static FloatingTextManager instance;
    public GameObject textPrefab;

    // YENÝ: Bu deðeri deðiþtirerek yazýnýn ne kadar yukarýda çýkacaðýný ayarlayabilirsin.
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

        // Dünya pozisyonunu ekran pozisyonuna çeviriyoruz.
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(position);

        // YENÝ: Hesaplanan ekran pozisyonuna dikey bir ofset ekliyoruz.
        screenPosition.y += verticalOffset;

        textObject.transform.position = screenPosition;

        TextMeshProUGUI tmp = textObject.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.color = color;

        // BU SATIRI SÝLÝYORUZ, çünkü artýk FloatingText.cs bu iþi yapýyor.
        // Destroy(textObject, 0.8f); 
    }
}