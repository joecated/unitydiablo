// ArcherAI.cs (HATALAR D�ZELT�LD�)
// Menzilli sald�r� yapan d��man�n yapay zekas�.
using UnityEngine;
using UnityEngine.AI;

public class ArcherAI : MonoBehaviour
{
    [Header("AI Ayarlar�")]
    public float lookRadius = 15f; // D��man�n oyuncuyu fark edece�i mesafe

    [Header("Sava� Ayarlar�")]
    public GameObject arrowPrefab;
    public Transform firePoint;

    // Attack Speed (Sald�r� H�z�) YORUMU:
    // Bu de�er saniyedeki sald�r� say�s�n� belirtir.
    // 1 = Saniyede 1 sald�r�.
    // 0.5 = 2 saniyede 1 sald�r�. (Daha yava�)
    // 2 = Saniyede 2 sald�r�. (Daha h�zl�)
    // De�eri Inspector'dan d���rerek ok�uyu yava�latabilirsin.
    public float attackSpeed = 1f;
    private float attackCooldown = 0f;

    // Gerekli Bile�enler
    private Transform target;
    private NavMeshAgent agent;
    private CharacterStats targetStats;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = PlayerStats.instance.transform;
        if (target != null)
        {
            targetStats = target.GetComponent<CharacterStats>();
        }

        if (firePoint == null)
        {
            firePoint = transform;
        }
    }

    void Update()
    {
        if (target == null || targetStats.currentHealth <= 0)
        {
            agent.isStopped = true;
            return;
        }

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }

        float distance = Vector3.Distance(target.position, transform.position);

        // E�er oyuncu g�r�� mesafesinin d���ndaysa, dur ve bekle.
        if (distance > lookRadius)
        {
            agent.isStopped = true;
            return;
        }

        // E�er oyuncu sald�r� menzilindeyse, dur ve sald�r.
        if (distance <= agent.stoppingDistance)
        {
            agent.isStopped = true; // Hareketi durdur
            FaceTarget();
            if (attackCooldown <= 0f)
            {
                Attack();
                attackCooldown = 1f / attackSpeed;
            }
        }
        // E�er oyuncu g�r�� mesafesinde ama sald�r� menzili d���ndaysa, takip et.
        else
        {
            agent.isStopped = false; // Harekete devam et
            agent.SetDestination(target.position);
        }
    }

    void Attack()
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        Projectile projectile = arrow.GetComponent<Projectile>();

        if (projectile != null)
        {
            projectile.Seek(target);
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}