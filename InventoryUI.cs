// InventoryUI.cs (Envanter Aray�z Y�neticisi)
using UnityEngine;
using TMPro; // TextMeshPro kullanmak i�in bu sat�r gerekli

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryUIPanel;
    public Transform itemsParent;
    public TextMeshProUGUI goldText; // YEN�: Alt�n metni i�in referans
    public static InventoryUI instance; // YEN�
    InventorySlot[] slots;
    Inventory inventory;

    void Awake() // YEN�
    {
        instance = this;
    }
    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;
        // YEN�: PlayerStats'in alt�n sinyalini dinlemeye ba�la
        PlayerStats.instance.OnGoldChanged += UpdateGoldUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();


        inventoryUIPanel.SetActive(false);
        UpdateGoldUI(PlayerStats.instance.gold); // Ba�lang�� de�erini ata
    }

    void OnDestroy() // Obje yok oldu�unda event aboneli�ini iptal etmeyi unutma
    {
        PlayerStats.instance.OnGoldChanged -= UpdateGoldUI;
    }
    // YEN� FONKS�YON
    void UpdateGoldUI(int currentGold)
    {
        if (goldText != null)
        {
            goldText.text = "Alt�n: " + currentGold.ToString();
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