// Portal.cs
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class Portal : MonoBehaviour
{
    [Tooltip("Gidilecek sahnenin tam ad�n� yaz�n (Build Settings'teki gibi).")]
    public string destinationSceneName;

    void Awake()
    {
        // Portal�n trigger olarak ayarland���ndan emin ol.
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (string.IsNullOrEmpty(destinationSceneName))
        {
            Debug.LogError("HATA: Portalda hedef sahne ad� belirtilmemi�!", this.gameObject);
            return;
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("Oyuncu portala girdi. Sahne y�kleniyor: " + destinationSceneName);
            // NOT: Daha geli�mi� sistemlerde burada bir y�kleme ekran� g�sterilebilir.
            SceneManager.LoadScene(destinationSceneName);
        }
    }
}