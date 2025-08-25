// EnemyController.cs (G�NCELLENM�� VE EN SA�LAM HAL�)
using UnityEngine;
using UnityEngine.AI;

// --- YEN�: Kitle Kontrol tiplerini burada tan�ml�yoruz ---
public enum CrowdControlType
{
    None,
    Stun
}

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterStats))]
public class EnemyController : MonoBehaviour
{
    [Header("Davran�� Ayarlar�")]
    public float lookRadius = 10f;

    [Header("Sald�r� Ayarlar�")]
    public int attackDamage = 5;
    public float attackSpeed = 1f;

    // --- Private De�i�kenler ---
    private float attackCooldown = 0f;
    private Transform target;
    private CharacterStats targetStats;
    private NavMeshAgent agent;
    private bool isDead = false;

    // --- YEN�: Sersemletme (Stun) i�in gereken de�i�kenler ---
    private CrowdControlType currentCC = CrowdControlType.None;
    private float ccTimer = 0f;
    public bool IsStunned => currentCC == CrowdControlType.Stun;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Oyuncu sahnede oldu�u i�in bu kod art�k g�venle �al���r.
        target = GameObject.FindGameObjectWithTag("Player").transform;
        targetStats = target.GetComponent<CharacterStats>();
    }

    void Update()
    {
        // E�er �lm��sek, hi�bir �ey yapma.
        if (isDead) return;

        // --- YEN�: SERSEMLETME KONTROL� ---
        // Update fonksiyonunun en ba��na bu kontrol� ekliyoruz.
        // E�er sersemlemi� durumdaysak, zamanlay�c�y� �al��t�r ve ba�ka hi�bir �ey yapma.
        if (IsStunned)
        {
            ccTimer -= Time.deltaTime;
            if (ccTimer <= 0)
            {
                ClearCC(); // S�re bitti, sersemletmeyi kald�r.
            }
            return; // Fonksiyonun geri kalan�n� �al��t�rma!
        }
        // --- YEN� KOD B�T��� ---

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

    // --- BU FONKS�YONLARA H�� DOKUNULMADI ---
    void Attack()
    {
        FaceTarget();
        if (attackCooldown <= 0f)
        {
            Debug.Log("<color=orange>" + gameObject.name + " Sald�r�yor!</color>");
            targetStats.TakeDamage(attackDamage, DamageType.Physical);
            attackCooldown = 1f / attackSpeed;
        }
    }

    // --- BU FONKS�YONLARA H�� DOKUNULMADI ---
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // --- BU FONKS�YONLARA H�� DOKUNULMADI ---
    //public void Die()
    //{
    //    if (isDead) return;
    //    isDead = true;

    //    Debug.Log("<color=red>" + transform.name + " �ld�.</color>");

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

    // --- YEN�: CC UYGULAMA VE TEM�ZLEME FONKS�YONLARI ---

    /// <summary>
    /// Bu d��mana belirtilen s�re boyunca Stun uygular.
    /// </summary>
    /// <param name="duration">Sersemletme s�resi (saniye).</param>
    public void ApplyStun(float duration)
    {
        if (isDead) return; // �l� d��man sersemlemez.

        currentCC = CrowdControlType.Stun;
        ccTimer = duration;

        // Davran��lar� an�nda durdur
        if (agent != null)
        {
            agent.isStopped = true; // Hareketi durdur
            agent.velocity = Vector3.zero; // Anl�k h�z� s�f�rla
        }

        // Burada animasyon tetikleyicisi veya UI g�sterme kodlar� da olabilir.

        Debug.Log(gameObject.name + " " + duration + " saniyeli�ine SERSEMLET�LD�!");
    }

    /// <summary>
    /// D��man �zerindeki t�m CC etkilerini temizler.
    /// </summary>
    private void ClearCC()
    {
        currentCC = CrowdControlType.None;
        ccTimer = 0f;

        // Davran��lar� normale d�nd�r
        if (agent != null && agent.enabled)
        {
            agent.isStopped = false; // Harekete tekrar izin ver
        }

        // Burada UI gizleme kodlar� olabilir.

        Debug.Log(gameObject.name + " normale d�nd�.");
    }

    // Die() fonksiyonun burada olmasa da, e�er olsayd� isDead = true; sat�r�n� eklerdik.
    // Senin yap�nda Die() EnemyStats'ta oldu�u i�in �imdilik bir sorun yok.
}