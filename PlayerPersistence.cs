using UnityEngine;

public class PlayerPersistence : MonoBehaviour
{
    // 'static' bir deðiþken, bu script'in tüm kopyalarý tarafýndan paylaþýlýr.
    // Bu sayede oyunda bizden bir tane mi var, yoksa daha önce biri gelmiþ mi anlayabiliriz.
    public static PlayerPersistence instance;

    private void Awake()
    {
        // Eðer 'instance' daha önce hiç ayarlanmadýysa (yani bu oyuna giren ilk oyuncuysa)...
        if (instance == null)
        {
            // Bu objeyi 'instance' olarak ata. Yani "patron benim" de.
            instance = this;
            // Ve bu objeyi sahneler arasý taþýmasý için iþaretle.
            DontDestroyOnLoad(gameObject);
        }
        // Eðer 'instance' zaten varsa VE bu obje o instance deðilse...
        else if (instance != this)
        {
            // Bu, sonradan gelen kopya demektir.
            Debug.Log("Kopya bir oyuncu (" + gameObject.name + ") tespit edildi ve yok ediliyor.");
            // Kendini hemen yok et.
            Destroy(gameObject);
        }
    }
}