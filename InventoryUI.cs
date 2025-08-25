// InventoryUI.cs (Envanter Arayüz Yöneticisi)
using UnityEngine;
using TMPro; // TextMeshPro kullanmak için bu satýr gerekli

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryUIPanel;
    public Transform itemsParent;
    public TextMeshProUGUI goldText; // YENÝ: Altýn metni için referans
    public static InventoryUI instance; // YENÝ
    InventorySlot[] slots;
    Inventory inventory;

    void Awake() // YENÝ
    {
        instance = this;
    }
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
        // YENÝ: PlayerStats'in altýn sinyalini dinlemeye baþla
        PlayerStats.instance.OnGoldChanged += UpdateGoldUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();


        inventoryUIPanel.SetActive(false);
        UpdateGoldUI(PlayerStats.instance.gold); // Baþlangýç deðerini ata
    }

    void OnDestroy() // Obje yok olduðunda event aboneliðini iptal etmeyi unutma
    {
        PlayerStats.instance.OnGoldChanged -= UpdateGoldUI;
    }
    // YENÝ FONKSÝYON
    void UpdateGoldUI(int currentGold)
    {
        if (goldText != null)
        {
            goldText.text = "Altýn: " + currentGold.ToString();
        }
    }


    

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}