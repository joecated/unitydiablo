// PlayerController.cs (DO�RU REFERANS ���N F�NAL VERS�YON)
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour // BU SCRIPT Player_ROOT �ZER�NDE
{
    // YEN�: Bedenimize ait bile�enler i�in public referanslar
    [Header("Beden Par�alar� (Model Objesinden S�r�klenecek)")]
    public CharacterController characterController;
    public Animator animator;


    public event System.Action<Transform> OnNewTargetSelected;


    private Camera mainCamera;
    private NavMeshAgent agent;
    private Transform target;
    private PlayerCombat playerCombat;

    private float gravity = -9.81f;
    private Vector3 playerVelocity;

    void Awake()
    {
        // Kendi �zerimizdeki beyin bile�enlerini al�yoruz
        agent = GetComponent<NavMeshAgent>();
        playerCombat = GetComponent<PlayerCombat>();
        mainCamera = Camera.main;
        // D�KKAT: Animator ve CharacterController'� buradan kald�rd�k, ��nk� art�k Inspector'dan at�yoruz.
    }

    void Start()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
    }


    void Update()
    {
        // --- B�L�M 1: ARAY�Z A�MA/KAPAMA INPUTLARI (EN Y�KSEK �NCEL�K) ---
        // Bu tu�lar, ba�ka bir panel a��k olsa bile her zaman dinlenmeli.

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryUI.instance != null)
            {
                // Envanterin mevcut durumunu al ve tam tersini yap (toggle)
                bool isActive = InventoryUI.instance.inventoryUIPanel.activeSelf;
                InventoryUI.instance.inventoryUIPanel.SetActive(!isActive);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (QuestPanelUI.instance != null)
                QuestPanelUI.instance.TogglePanel();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // A��k olan t�m panelleri kapat
            if (QuestPanelUI.instance != null) QuestPanelUI.instance.ClosePanel();
            if (ShopUI.instance != null) ShopUI.instance.CloseShop();
            if (BlacksmithUI.instance != null) BlacksmithUI.instance.CloseBlacksmith();
            if (InventoryUI.instance != null) InventoryUI.instance.inventoryUIPanel.SetActive(false);

            // Odaklanm�� bir buton varsa oda�� kald�r
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }


        


        // --- B�L�M 3: OYUN ��� AKS�YONLAR (E�er t�m paneller kapal�ysa) ---

        HandleInputAndTargeting();
        MoveCharacter();
        UpdateAnimator();
    }

    void MoveCharacter()
    {
        if (characterController.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        Vector3 move = agent.desiredVelocity;
        characterController.Move(move * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
        transform.position = characterController.transform.position;
        agent.nextPosition = transform.position;
        if (move.sqrMagnitude > 0.1f)
        {
            characterController.transform.rotation = Quaternion.LookRotation(move.normalized);
        }
    }

    void UpdateAnimator()
    {
        // Referans�n bo� olup olmad���n� kontrol edelim, emin olmak i�in.
        if (animator == null)
        {
            Debug.LogError("PlayerController'daki Animator referans� BO�!");
            return;
        }
        if (characterController == null)
        {
            Debug.LogError("PlayerController'daki CharacterController referans� BO�!");
            return;
        }

        // Fiziksel h�z� hesapla
        float speed = agent.desiredVelocity.magnitude / agent.speed;

        // --- TEST ���N BU SATIRI EKLE ---
        // Konsola, Animator'e hangi h�z de�erini g�nderdi�imizi yazd�ral�m.
        //Debug.Log("Animator'e g�nderilen Speed de�eri: " + speed);

        // Animator'deki "Speed" parametresini g�ncelle
        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    void FaceTarget()
    {
        if (target == null) return;
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        characterController.transform.rotation = Quaternion.Slerp(characterController.transform.rotation, lookRotation, Time.deltaTime * 20f);
    }

    #region Dokunulmayan Fonksiyonlar
    public Transform GetTarget() { return target; }
    void HandleInputAndTargeting()
    {
        // --- YEN� G�VENL�K KONTROL� ---
        // E�er fare imleci herhangi bir UI eleman�n�n (panel, buton vb.) �zerindeyse,
        // bu fonksiyondan hemen ��k ve oyun d�nyas�nda bir �ey se�meye/y�r�meye �al��ma.
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        // --- KONTROL B�TT� ---


        if ((QuestPanelUI.instance != null && QuestPanelUI.instance.GetComponent<CanvasGroup>().interactable) ||
            (ShopUI.instance != null && ShopUI.instance.GetComponent<CanvasGroup>().interactable) ||
            (BlacksmithUI.instance != null && BlacksmithUI.instance.GetComponent<CanvasGroup>().interactable))
        {
            agent.ResetPath(); return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Transform newTarget = null; // Ge�ici de�i�ken
                if (hit.collider.CompareTag("Enemy")) { newTarget = hit.transform; ; }
                // YEN�: E�er hedefimiz de�i�tiyse, sinyali g�nder
                if (newTarget != target)
                {
                    target = newTarget;
                    OnNewTargetSelected?.Invoke(target);
                }
                
                agent.SetDestination(hit.point);
            }
        }
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            float attackRange = (playerCombat.basicAttackSkill != null) ? playerCombat.basicAttackSkill.range : 2f;
            if (distance > attackRange) { agent.SetDestination(target.position); }
            else { agent.ResetPath(); FaceTarget(); playerCombat.DoBasicAttack(); }
        }
    }
    #endregion
}