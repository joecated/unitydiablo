using UnityEngine;

public class PlayerPersistence : MonoBehaviour
{
    // 'static' bir de�i�ken, bu script'in t�m kopyalar� taraf�ndan payla��l�r.
    // Bu sayede oyunda bizden bir tane mi var, yoksa daha �nce biri gelmi� mi anlayabiliriz.
    public static PlayerPersistence instance;

    private void Awake()
    {
        // E�er 'instance' daha �nce hi� ayarlanmad�ysa (yani bu oyuna giren ilk oyuncuysa)...
        if (instance == null)
        {
            // Bu objeyi 'instance' olarak ata. Yani "patron benim" de.
            instance = this;
            // Ve bu objeyi sahneler aras� ta��mas� i�in i�aretle.
            DontDestroyOnLoad(gameObject);
        }
        // E�er 'instance' zaten varsa VE bu obje o instance de�ilse...
        else if (instance != this)
        {
            // Bu, sonradan gelen kopya demektir.
            Debug.Log("Kopya bir oyuncu (" + gameObject.name + ") tespit edildi ve yok ediliyor.");
            // Kendini hemen yok et.
            Destroy(gameObject);
        }
    }
}