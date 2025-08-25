// HydraController.cs (S�PER-KONU�KAN DEBUG VERS�YONU)
using UnityEngine;

public class HydraController : MonoBehaviour
{
    [Header("Ayarlar")]
    public float duration = 10f;
    public float detectionRadius = 15f;
    public float attackSpeed = 1f;
    public int projectileDamage = 5;

    [Header("Referanslar")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    private float attackCooldown = 0f;
    private Transform currentTarget;
    private int damagePerShot; // Art�k hasar� d��ar�dan alacak

    // YEN� FONKS�YON: Hydra yarat�ld���nda hasar�n� belirlemek i�in
    public void Initialize(int damage)
    {
        damagePerShot = damage;
    }

    void Start()
    {
        Destroy(gameObject, duration);
    }

    void Update()
    {
        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
        FindTargetAndAttack();
    }

    void FindTargetAndAttack()
    {
        if (currentTarget != null && (Vector3.Distance(transform.position, currentTarget.position) > detectionRadius || currentTarget.GetComponent<CharacterStats>().currentHealth <= 0))
        {
            currentTarget = null;
        }

        if (currentTarget == null)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy"))
                {
                    Debug.Log("!!! D��MAN BULUNDU: " + hit.gameObject.name + " !!!");
                    currentTarget = hit.transform;
                    break;
                }
            }
        }

        // --- YEN� TEST KODLARI BURADA ---
        if (currentTarget != null)
        {
            Debug.Log("Hedef kilitli: " + currentTarget.name + ". Ate� etme kontrol ediliyor. Cooldown: " + attackCooldown);
            if (attackCooldown <= 0)
            {
                Debug.Log("!!! ATE� ETME KOMUTU VER�LD� !!!");
                FireProjectile(currentTarget);
                attackCooldown = 1f / attackSpeed;
            }
        }
    }

    void FireProjectile(Transform target)
    {
        if (projectilePrefab == null || firePoint == null) return;
        GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();

        if (projectile != null)
        {
            // Art�k hasar� kendi i�indeki damagePerShot de�i�keninden al�yor
            projectile.damage = this.damagePerShot;
            projectile.Seek(target);
        }
    }
}