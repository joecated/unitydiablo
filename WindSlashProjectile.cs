// WindSlashProjectile.cs (YENÝ SCRÝPT)
// Riven'in R'sinin ikinci kýsmýnýn mermisini yönetir.
using UnityEngine;

// public class WindSlashProjectile : MonoBehaviour satýrý, bu script'in sahnedeki bir objeye eklenebilmesini saðlar.
public class WindSlashProjectile : MonoBehaviour
{
    // public float speed... satýrý, merminin ne kadar hýzlý gideceðini belirler.
    public float speed = 20f;
    // public int damage... satýrý, merminin hasarýný belirler.
    public int damage = 0;
    // public float lifeTime... satýrý, merminin ne kadar süre sonra kendini yok edeceðini belirler.
    public float lifeTime = 3f;

    // Awake fonksiyonu, obje oluþturulduðunda bir kere çalýþýr.
    void Awake()
    {
        // Destroy(gameObject, lifeTime); satýrý, bu objeyi 'lifeTime' saniye sonra otomatik olarak yok eder. Bu, merminin sonsuza kadar gitmesini engeller.
        Destroy(gameObject, lifeTime);
    }

    // Update fonksiyonu, her karede sürekli çalýþýr.
    void Update()
    {
        // transform.Translate(Vector3.forward * speed * Time.deltaTime); satýrý, mermiyi her karede kendi "ileri" yönünde 'speed' hýzýyla hareket ettirir.
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // OnTriggerEnter fonksiyonu, bu objenin tetikleyici (trigger) collider'ý baþka bir collider'a deðdiðinde çalýþýr.
    void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("Enemy")) satýrý, çarptýðý objenin bir "Enemy" (Düþman) olup olmadýðýný kontrol eder.
        if (other.CompareTag("Enemy"))
        {
            // CharacterStats stats = other.GetComponent<CharacterStats>(); satýrý, çarptýðý düþmanýn canýný yöneten 'CharacterStats' script'ini bulur.
            CharacterStats stats = other.GetComponent<CharacterStats>();
            // if (stats != null) satýrý, script'in bulunduðundan emin olur.
            if (stats != null)
            {
                // --- HATA DÜZELTMESÝ BURADA ---
                // 1. Hasarý Mathf.RoundToInt ile en yakýn tam sayýya yuvarlýyoruz.
                // 2. Hasar türünü (örneðin Magical) ikinci parametre olarak veriyoruz.
                stats.TakeDamage(Mathf.RoundToInt(damage), DamageType.Magical);
            }
        }
    }
}