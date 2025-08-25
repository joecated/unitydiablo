// Projectile.cs (YENÝ SCRÝPT)
// Ok gibi mermilerin davranýþýný kontrol eder.
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    public float speed = 15f;
    public int damage = 10;

    // Mermiye hedefini söyleyen fonksiyon
    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        // Eðer hedef yoksa, mermiyi yok et
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Hedefe doðru yönü ve mesafeyi hesapla
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // Eðer bu karede hedefe ulaþacaksa...
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // Hedefe doðru hareket et
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        // Okun yüzünü hedefe döndür
        transform.LookAt(target);
    }

    void HitTarget()
    {
        // Hedefin canýný azalt
        CharacterStats targetStats = target.GetComponent<CharacterStats>();
        if (targetStats != null)
        {
            targetStats.TakeDamage(damage, DamageType.Physical);
        }

        // Mermiyi yok et
        Destroy(gameObject);
    }
}
