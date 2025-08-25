// Tooltip.cs (NULL REFERANS HATASI ���N N�HA� ��Z�M)
// A��klama panelinin kendisini y�netir.
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerText;
    public TextMeshProUGUI contentText;
    public TextMeshProUGUI manaCostText;
    public TextMeshProUGUI cooldownText;

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        // N�HA� ��Z�M: Referanslar� Inspector'a g�venmek yerine kodla otomatik olarak bul.
        // Bu, Unity'nin referanslar� kaybetme sorununu k�k�nden ��zer.
        if (headerText == null) headerText = transform.Find("Header_Section/Header_Text")?.GetComponent<TextMeshProUGUI>();
        if (contentText == null) contentText = transform.Find("Content_Text")?.GetComponent<TextMeshProUGUI>();
        if (manaCostText == null) manaCostText = transform.Find("Header_Section/Stats_Section/ManaCost_Text")?.GetComponent<TextMeshProUGUI>();
        if (cooldownText == null) cooldownText = transform.Find("Header_Section/Stats_Section/Cooldown_Text")?.GetComponent<TextMeshProUGUI>();

        // Paneli ba�lang��ta gizle
        Hide();
    }

    // ARTIK FAREY� TAK�P ETMEMES� ���N UPDATE FONKS�YONU S�L�ND�.

    public void SetText(string header, string content, string manaCost, string cooldown)
    {
        // HATA KONTROL�: E�er referanslar hala bulunamad�ysa, hata ver ve devam etme.
        if (headerText == null || contentText == null || manaCostText == null || cooldownText == null)
        {
            Debug.LogError("HATA: Tooltip_Panel'in alt�ndaki yaz� objelerinin isimleri veya hiyerar�isi yanl��! L�tfen kurulum rehberini kontrol et.");
            return;
        }

        headerText.text = header;
        contentText.text = content;
        manaCostText.text = manaCost;
        cooldownText.text = cooldown;

        headerText.gameObject.SetActive(!string.IsNullOrEmpty(header));
        contentText.gameObject.SetActive(!string.IsNullOrEmpty(content));
        manaCostText.gameObject.SetActive(!string.IsNullOrEmpty(manaCost));
        cooldownText.gameObject.SetActive(!string.IsNullOrEmpty(cooldown));
    }

    public void Show()
    {
        canvasGroup.alpha = 1f;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
    }
}