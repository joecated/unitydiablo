// BlacksmithUI.cs (CANVAS GROUP ÝLE ÇALIÞAN FÝNAL VERSÝYON)
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Text;

public class BlacksmithUI : MonoBehaviour
{
    public static BlacksmithUI instance;

    [Header("Panel Referanslarý")]
    public GameObject blacksmithPanel; // Bu referans kalabilir ama artýk direct kontrol etmeyeceðiz
    public Transform playerItemsContainer;
    public GameObject blacksmithSlotPrefab;

    [Header("Demirci Ocaðý UI")]
    public Image itemToUpgradeIcon;
    public Image resultIcon;
    public TextMeshProUGUI requirementsText;
    public Button upgradeButton;

    private Item currentItemToUpgrade;
    private UpgradeRecipe currentRecipe;
    private UpgradeRecipe[] allRecipes;

    private CanvasGroup canvasGroup; // YENÝ: Canvas Group referansý

    void Awake()
    {
        instance = this;
        allRecipes = Resources.LoadAll<UpgradeRecipe>("UpgradeRecipes");

        // Canvas Group bileþenini bul ve referans al
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            // Eðer yoksa, bir tane otomatik ekle (güvenlik önlemi)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Start()
    {
        upgradeButton.onClick.AddListener(OnUpgradeButtonClicked);
        // Paneli baþlangýçta gizle (yeni yöntemle)
        CloseBlacksmith();
    }

    public void OpenBlacksmith()
    {
        // Paneli görünür ve etkileþilebilir yap
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        RedrawPlayerInventory();
        ClearForge();
    }

    public void CloseBlacksmith()
    {
        // Paneli görünmez ve etkileþimsiz yap
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

   

    // --- GERÝ KALAN TÜM FONKSÝYONLARIN BÝREBÝR AYNI KALIYOR ---
    // OnPlayerItemClicked, DisplayRecipe, OnUpgradeButtonClicked, vb.
    #region Dokunulmayan Fonksiyonlar
    public void OnPlayerItemClicked(Item item)
    {
        currentItemToUpgrade = item;
        itemToUpgradeIcon.sprite = item.icon;
        itemToUpgradeIcon.enabled = true;
        currentRecipe = FindRecipeForItem(item);
        if (currentRecipe != null)
        {
            DisplayRecipe(currentRecipe);
        }
        else
        {
            ClearForge(false);
        }
    }
    void DisplayRecipe(UpgradeRecipe recipe)
    {
        resultIcon.sprite = recipe.upgradedItemResult.icon;
        resultIcon.enabled = true;
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<b>Gerekenler:</b>");
        bool canCraft = true;
        if (!Inventory.instance.HasMaterials(recipe.materialsRequired))
        {
            canCraft = false;
        }
        foreach (var cost in recipe.materialsRequired)
        {
            int owned = Inventory.instance.items.FindAll(x => x.name == cost.material.name).Count;
            string color = (owned >= cost.quantity) ? "#5DFF5D" : "#FF5D5D";
            sb.AppendLine($"<color={color}>- {cost.material.name}: {owned} / {cost.quantity}</color>");
        }
        if (PlayerStats.instance.gold < recipe.goldCost)
        {
            canCraft = false;
        }
        string goldColor = (PlayerStats.instance.gold >= recipe.goldCost) ? "#5DFF5D" : "#FF5D5D";
        sb.AppendLine($"<color={goldColor}>- Altýn: {PlayerStats.instance.gold} / {recipe.goldCost}</color>");
        requirementsText.text = sb.ToString();
        upgradeButton.interactable = canCraft;
    }
    void OnUpgradeButtonClicked()
    {
        if (currentRecipe != null && currentItemToUpgrade != null)
        {
            if (Inventory.instance.HasMaterials(currentRecipe.materialsRequired) && PlayerStats.instance.SpendGold(currentRecipe.goldCost))
            {
                Inventory.instance.Remove(currentItemToUpgrade);
                Inventory.instance.ConsumeMaterials(currentRecipe.materialsRequired);
                Inventory.instance.Add(currentRecipe.upgradedItemResult);
                Debug.Log(currentItemToUpgrade.name + " baþarýyla " + currentRecipe.upgradedItemResult.name + " haline getirildi!");
                RedrawPlayerInventory();
                ClearForge();
            }
        }
    }
    void ClearForge(bool clearSelectedItem = true)
    {
        if (clearSelectedItem)
        {
            currentItemToUpgrade = null;
            itemToUpgradeIcon.sprite = null;
            itemToUpgradeIcon.enabled = false;
        }
        currentRecipe = null;
        resultIcon.sprite = null;
        resultIcon.enabled = false;
        requirementsText.text = "Geliþtirmek için bir eþya seç...";
        upgradeButton.interactable = false;
    }
    public void RedrawPlayerInventory()
    {
        foreach (Transform child in playerItemsContainer) { Destroy(child.gameObject); }
        foreach (Item item in Inventory.instance.items)
        {
            GameObject slotObj = Instantiate(blacksmithSlotPrefab, playerItemsContainer);
            slotObj.GetComponent<BlacksmithSlotUI>().Setup(item);
        }
    }
    UpgradeRecipe FindRecipeForItem(Item item)
{
    foreach (var recipe in allRecipes)
    {
        // GÜVENLÝK ÖNLEMÝ: Eðer tarifin içi boþsa veya geliþtirilecek eþya atanmamýþsa,
        // bu adýmý atla ve konsola bir uyarý yaz.
        if (recipe == null || recipe.itemToUpgrade == null)
        {
            Debug.LogWarning("Resources/UpgradeRecipes klasöründe içi boþ veya hatalý bir tarif bulundu!");
            continue; // Bu tarifi atla, bir sonrakine geç.
        }

        if (recipe.itemToUpgrade.name == item.name)
        {
            return recipe;
        }
    }
    return null;
}
    #endregion
}