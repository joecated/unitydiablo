// Portal.cs
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class Portal : MonoBehaviour
{
    [Tooltip("Gidilecek sahnenin tam adýný yazýn (Build Settings'teki gibi).")]
    public string destinationSceneName;

    void Awake()
    {
        // Portalýn trigger olarak ayarlandýðýndan emin ol.
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (string.IsNullOrEmpty(destinationSceneName))
        {
            Debug.LogError("HATA: Portalda hedef sahne adý belirtilmemiþ!", this.gameObject);
            return;
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("Oyuncu portala girdi. Sahne yükleniyor: " + destinationSceneName);
            // NOT: Daha geliþmiþ sistemlerde burada bir yükleme ekraný gösterilebilir.
            SceneManager.LoadScene(destinationSceneName);
        }
    }
}