// ArcherAI.cs (HATALAR DÜZELTÝLDÝ)
// Menzilli saldýrý yapan düþmanýn yapay zekasý.
using UnityEngine;
using UnityEngine.AI;

public class ArcherAI : MonoBehaviour
{
    [Header("AI Ayarlarý")]
    public float lookRadius = 15f; // Düþmanýn oyuncuyu fark edeceði mesafe

    [Header("Savaþ Ayarlarý")]
    public GameObject arrowPrefab;
    public Transform firePoint;

    // Attack Speed (Saldýrý Hýzý) YORUMU:
    // Bu deðer saniyedeki saldýrý sayýsýný belirtir.
    // 1 = Saniyede 1 saldýrý.
    // 0.5 = 2 saniyede 1 saldýrý. (Daha yavaþ)
    // 2 = Saniyede 2 saldýrý. (Daha hýzlý)
    // Deðeri Inspector'dan düþürerek okçuyu yavaþlatabilirsin.
    public float attackSpeed = 1f;
    private float attackCooldown = 0f;

    // Gerekli Bileþenler
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

        // Eðer oyuncu görüþ mesafesinin dýþýndaysa, dur ve bekle.
        if (distance > lookRadius)
        {
            agent.isStopped = true;
            return;
        }

        // Eðer oyuncu saldýrý menzilindeyse, dur ve saldýr.
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
        // Eðer oyuncu görüþ mesafesinde ama saldýrý menzili dýþýndaysa, takip et.
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