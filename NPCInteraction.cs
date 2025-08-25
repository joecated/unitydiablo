// NPCInteraction.cs (HIGHLIGHT RING PREFAB SÝSTEMÝ)
using UnityEngine;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    public enum NPCType { Shopkeeper, Blacksmith, QuestGiver }

    [Header("NPC Ayarlarý")]
    public NPCType type;
    public string npcName = "NPC";
    public float interactionDistance = 4f;

    // YENÝ: Eðer bu bir tüccarsa, hangi mallarý sattýðýný buraya atayacaðýz.
    [Header("Dükkan Ayarlarý (Sadece Tüccar için)")]
    public ShopData shopData;

    [Header("Görsel Efektler")]
    public GameObject highlightRingPrefab; // YENÝ: Projector yerine halka prefab'ý
    public GameObject nameplatePrefab;

    private GameObject highlightInstance; // Sahnede oluþturduðumuz halkanýn kopyasý
    private GameObject nameplateInstance;

    // Start fonksiyonu artýk boþ, silebilirsin veya býrakabilirsin.

    void OnMouseEnter()
    {
        // YENÝ: Halka prefab'ýný NPC'nin pozisyonunda yarat
        if (highlightRingPrefab != null && highlightInstance == null)
        {
            // Pozisyonu NPC'nin transform'uyla ayný yap ama Y'sini 0.1 gibi çok küçük bir deðer yap ki zeminin üstünde dursun.
            Vector3 spawnPosition = new Vector3(transform.position.x, 0.1f, transform.position.z);
            highlightInstance = Instantiate(highlightRingPrefab, spawnPosition, highlightRingPrefab.transform.rotation);
        }

        if (nameplatePrefab != null && nameplateInstance == null)
        {
            Vector3 nameplatePosition = transform.position + Vector3.up * 2.2f;
            nameplateInstance = Instantiate(nameplatePrefab, nameplatePosition, Quaternion.identity);
            nameplateInstance.GetComponentInChildren<TextMeshProUGUI>().text = npcName;
            nameplateInstance.transform.rotation = Camera.main.transform.rotation;
        }
    }

    void OnMouseExit()
    {
        // YENÝ: Oluþturduðumuz halkayý yok et
        if (highlightInstance != null)
        {
            Destroy(highlightInstance);
        }

        if (nameplateInstance != null)
        {
            Destroy(nameplateInstance);
        }
    }

    // ... OnMouseDown fonksiyonu ayný kalýyor ...
    #region OnMouseDown
    void OnMouseDown()
    {
        float distance = Vector3.Distance(PlayerStats.instance.transform.position, transform.position);
        if (distance <= interactionDistance)
        {
            switch (type)
            {
                case NPCType.Shopkeeper:
                    // Artýk doðrudan kendi üzerindeki ShopData'yý gönderiyor
                    ShopUI.instance.OpenShop(shopData);
                    break;
                case NPCType.Blacksmith:
                    BlacksmithUI.instance.OpenBlacksmith();
                    break;
                case NPCType.QuestGiver:
                    break;
            }
        }
        else
        {
            Debug.Log("NPC'den çok uzaktasýn!");
        }
    }
    #endregion
}