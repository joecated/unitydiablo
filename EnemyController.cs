// EnemyController.cs (GÜNCELLENMÝÞ VE EN SAÐLAM HALÝ)
using UnityEngine;
using UnityEngine.AI;

// --- YENÝ: Kitle Kontrol tiplerini burada tanýmlýyoruz ---
public enum CrowdControlType
{
    None,
    Stun
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour
{
    [Header("Davranýþ Ayarlarý")]
    public float lookRadius = 10f;

    [Header("Saldýrý Ayarlarý")]
    public int attackDamage = 5;
    public float attackSpeed = 1f;

    // --- Private Deðiþkenler ---
    private float attackCooldown = 0f;
    private Transform target;
    private CharacterStats targetStats;
    private NavMeshAgent agent;
    private bool isDead = false;

    // --- YENÝ: Sersemletme (Stun) için gereken deðiþkenler ---
    private CrowdControlType currentCC = CrowdControlType.None;
    private float ccTimer = 0f;
    public bool IsStunned => currentCC == CrowdControlType.Stun;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Oyuncu sahnede olduðu için bu kod artýk güvenle çalýþýr.
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetStats = target.GetComponent<CharacterStats>();
    }

    void Update()
    {
        // Eðer ölmüþsek, hiçbir þey yapma.
        if (isDead) return;

        // --- YENÝ: SERSEMLETME KONTROLÜ ---
        // Update fonksiyonunun en baþýna bu kontrolü ekliyoruz.
        // Eðer sersemlemiþ durumdaysak, zamanlayýcýyý çalýþtýr ve baþka hiçbir þey yapma.
        if (IsStunned)
        {
            ccTimer -= Time.deltaTime;
            if (ccTimer <= 0)
            {
                ClearCC(); // Süre bitti, sersemletmeyi kaldýr.
            }
            return; // Fonksiyonun geri kalanýný çalýþtýrma!
        }
        // --- YENÝ KOD BÝTÝÞÝ ---

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);
            if (distance <= agent.stoppingDistance)
            {
                Attack();
            }
        }
    }

    // --- BU FONKSÝYONLARA HÝÇ DOKUNULMADI ---
    void Attack()
    {
        FaceTarget();
        if (attackCooldown <= 0f)
        {
            Debug.Log("<color=orange>" + gameObject.name + " Saldýrýyor!</color>");
            targetStats.TakeDamage(attackDamage, DamageType.Physical);
            attackCooldown = 1f / attackSpeed;
        }
    }

    // --- BU FONKSÝYONLARA HÝÇ DOKUNULMADI ---
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // --- BU FONKSÝYONLARA HÝÇ DOKUNULMADI ---
    //public void Die()
    //{
    //    if (isDead) return;
    //    isDead = true;

    //    Debug.Log("<color=red>" + transform.name + " öldü.</color>");

    //    if (agent != null)
    //    {
    //        agent.enabled = false;
    //    }
    //    GetComponent<Collider>().enabled = false;

    //    DungeonManager dungeonManager = FindObjectOfType<DungeonManager>();
    //    if (dungeonManager != null)
    //    {
    //        if (gameObject.CompareTag("Boss"))
    //        {
    //            dungeonManager.OnBossKilled();
    //        }
    //        else
    //        {
    //            dungeonManager.OnEnemyKilled();
    //        }
    //    }

    //    Destroy(gameObject, 2f);
    //}

    // --- YENÝ: CC UYGULAMA VE TEMÝZLEME FONKSÝYONLARI ---

    /// <summary>
    /// Bu düþmana belirtilen süre boyunca Stun uygular.
    /// </summary>
    /// <param name="duration">Sersemletme süresi (saniye).</param>
    public void ApplyStun(float duration)
    {
        if (isDead) return; // Ölü düþman sersemlemez.

        currentCC = CrowdControlType.Stun;
        ccTimer = duration;

        // Davranýþlarý anýnda durdur
        if (agent != null)
        {
            agent.isStopped = true; // Hareketi durdur
            agent.velocity = Vector3.zero; // Anlýk hýzý sýfýrla
        }

        // Burada animasyon tetikleyicisi veya UI gösterme kodlarý da olabilir.

        Debug.Log(gameObject.name + " " + duration + " saniyeliðine SERSEMLETÝLDÝ!");
    }

    /// <summary>
    /// Düþman üzerindeki tüm CC etkilerini temizler.
    /// </summary>
    private void ClearCC()
    {
        currentCC = CrowdControlType.None;
        ccTimer = 0f;

        // Davranýþlarý normale döndür
        if (agent != null && agent.enabled)
        {
            agent.isStopped = false; // Harekete tekrar izin ver
        }

        // Burada UI gizleme kodlarý olabilir.

        Debug.Log(gameObject.name + " normale döndü.");
    }

    // Die() fonksiyonun burada olmasa da, eðer olsaydý isDead = true; satýrýný eklerdik.
    // Senin yapýnda Die() EnemyStats'ta olduðu için þimdilik bir sorun yok.
}