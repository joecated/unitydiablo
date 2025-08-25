// PlayerController.cs (DOÐRU REFERANS ÝÇÝN FÝNAL VERSÝYON)
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour // BU SCRIPT Player_ROOT ÜZERÝNDE
{
    // YENÝ: Bedenimize ait bileþenler için public referanslar
    [Header("Beden Parçalarý (Model Objesinden Sürüklenecek)")]
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
        // Kendi üzerimizdeki beyin bileþenlerini alýyoruz
        agent = GetComponent<NavMeshAgent>();
        playerCombat = GetComponent<PlayerCombat>();
        mainCamera = Camera.main;
        // DÝKKAT: Animator ve CharacterController'ý buradan kaldýrdýk, çünkü artýk Inspector'dan atýyoruz.
    }

    void Start()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
    }


    void Update()
    {
        // --- BÖLÜM 1: ARAYÜZ AÇMA/KAPAMA INPUTLARI (EN YÜKSEK ÖNCELÝK) ---
        // Bu tuþlar, baþka bir panel açýk olsa bile her zaman dinlenmeli.

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
            // Açýk olan tüm panelleri kapat
            if (QuestPanelUI.instance != null) QuestPanelUI.instance.ClosePanel();
            if (ShopUI.instance != null) ShopUI.instance.CloseShop();
            if (BlacksmithUI.instance != null) BlacksmithUI.instance.CloseBlacksmith();
            if (InventoryUI.instance != null) InventoryUI.instance.inventoryUIPanel.SetActive(false);

            // Odaklanmýþ bir buton varsa odaðý kaldýr
            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        }


        


        // --- BÖLÜM 3: OYUN ÝÇÝ AKSÝYONLAR (Eðer tüm paneller kapalýysa) ---

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
        // Referansýn boþ olup olmadýðýný kontrol edelim, emin olmak için.
        if (animator == null)
        {
            Debug.LogError("PlayerController'daki Animator referansý BOÞ!");
            return;
        }
        if (characterController == null)
        {
            Debug.LogError("PlayerController'daki CharacterController referansý BOÞ!");
            return;
        }

        // Fiziksel hýzý hesapla
        float speed = agent.desiredVelocity.magnitude / agent.speed;

        // --- TEST ÝÇÝN BU SATIRI EKLE ---
        // Konsola, Animator'e hangi hýz deðerini gönderdiðimizi yazdýralým.
        //Debug.Log("Animator'e gönderilen Speed deðeri: " + speed);

        // Animator'deki "Speed" parametresini güncelle
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
        // --- YENÝ GÜVENLÝK KONTROLÜ ---
        // Eðer fare imleci herhangi bir UI elemanýnýn (panel, buton vb.) üzerindeyse,
        // bu fonksiyondan hemen çýk ve oyun dünyasýnda bir þey seçmeye/yürümeye çalýþma.
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        // --- KONTROL BÝTTÝ ---


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
                Transform newTarget = null; // Geçici deðiþken
                if (hit.collider.CompareTag("Enemy")) { newTarget = hit.transform; ; }
                // YENÝ: Eðer hedefimiz deðiþtiyse, sinyali gönder
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