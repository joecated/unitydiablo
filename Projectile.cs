// Projectile.cs (YEN� SCR�PT)
// Ok gibi mermilerin davran���n� kontrol eder.
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    public float speed = 15f;
    public int damage = 10;

    // Mermiye hedefini s�yleyen fonksiyon
    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        // E�er hedef yoksa, mermiyi yok et
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Hedefe do�ru y�n� ve mesafeyi hesapla
        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        // E�er bu karede hedefe ula�acaksa...
        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        // Hedefe do�ru hareket et
        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        // Okun y�z�n� hedefe d�nd�r
        transform.LookAt(target);
    }

    void HitTarget()
    {
        // Hedefin can�n� azalt
        CharacterStats targetStats = target.GetComponent<CharacterStats>();
        if (targetStats != null)
        {
            targetStats.TakeDamage(damage, DamageType.Physical);
        }

        // Mermiyi yok et
        Destroy(gameObject);
    }
}
