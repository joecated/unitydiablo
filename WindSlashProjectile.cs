// WindSlashProjectile.cs (YEN� SCR�PT)
// Riven'in R'sinin ikinci k�sm�n�n mermisini y�netir.
using UnityEngine;

// public class WindSlashProjectile : MonoBehaviour sat�r�, bu script'in sahnedeki bir objeye eklenebilmesini sa�lar.
public class WindSlashProjectile : MonoBehaviour
{
    // public float speed... sat�r�, merminin ne kadar h�zl� gidece�ini belirler.
    public float speed = 20f;
    // public int damage... sat�r�, merminin hasar�n� belirler.
    public int damage = 0;
    // public float lifeTime... sat�r�, merminin ne kadar s�re sonra kendini yok edece�ini belirler.
    public float lifeTime = 3f;

    // Awake fonksiyonu, obje olu�turuldu�unda bir kere �al���r.
    void Awake()
    {
        // Destroy(gameObject, lifeTime); sat�r�, bu objeyi 'lifeTime' saniye sonra otomatik olarak yok eder. Bu, merminin sonsuza kadar gitmesini engeller.
        Destroy(gameObject, lifeTime);
    }

    // Update fonksiyonu, her karede s�rekli �al���r.
    void Update()
    {
        // transform.Translate(Vector3.forward * speed * Time.deltaTime); sat�r�, mermiyi her karede kendi "ileri" y�n�nde 'speed' h�z�yla hareket ettirir.
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    // OnTriggerEnter fonksiyonu, bu objenin tetikleyici (trigger) collider'� ba�ka bir collider'a de�di�inde �al���r.
    void OnTriggerEnter(Collider other)
    {
        // if (other.CompareTag("Enemy")) sat�r�, �arpt��� objenin bir "Enemy" (D��man) olup olmad���n� kontrol eder.
        if (other.CompareTag("Enemy"))
        {
            // CharacterStats stats = other.GetComponent<CharacterStats>(); sat�r�, �arpt��� d��man�n can�n� y�neten 'CharacterStats' script'ini bulur.
            CharacterStats stats = other.GetComponent<CharacterStats>();
            // if (stats != null) sat�r�, script'in bulundu�undan emin olur.
            if (stats != null)
            {
                // --- HATA D�ZELTMES� BURADA ---
                // 1. Hasar� Mathf.RoundToInt ile en yak�n tam say�ya yuvarl�yoruz.
                // 2. Hasar t�r�n� (�rne�in Magical) ikinci parametre olarak veriyoruz.
                stats.TakeDamage(Mathf.RoundToInt(damage), DamageType.Magical);
            }
        }
    }
}