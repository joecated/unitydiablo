// NPCInteraction.cs (HIGHLIGHT RING PREFAB S�STEM�)
using UnityEngine;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    public enum NPCType { Shopkeeper, Blacksmith, QuestGiver }

    [Header("NPC Ayarlar�")]
    public NPCType type;
    public string npcName = "NPC";
    public float interactionDistance = 4f;

    // YEN�: E�er bu bir t�ccarsa, hangi mallar� satt���n� buraya atayaca��z.
    [Header("D�kkan Ayarlar� (Sadece T�ccar i�in)")]
    public ShopData shopData;

    [Header("G�rsel Efektler")]
    public GameObject highlightRingPrefab; // YEN�: Projector yerine halka prefab'�
    public GameObject nameplatePrefab;

    private GameObject highlightInstance; // Sahnede olu�turdu�umuz halkan�n kopyas�
    private GameObject nameplateInstance;

    // Start fonksiyonu art�k bo�, silebilirsin veya b�rakabilirsin.

    void OnMouseEnter()
    {
        // YEN�: Halka prefab'�n� NPC'nin pozisyonunda yarat
        if (highlightRingPrefab != null && highlightInstance == null)
        {
            // Pozisyonu NPC'nin transform'uyla ayn� yap ama Y'sini 0.1 gibi �ok k���k bir de�er yap ki zeminin �st�nde dursun.
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
        // YEN�: Olu�turdu�umuz halkay� yok et
        if (highlightInstance != null)
        {
            Destroy(highlightInstance);
        }

        if (nameplateInstance != null)
        {
            Destroy(nameplateInstance);
        }
    }

    // ... OnMouseDown fonksiyonu ayn� kal�yor ...
    #region OnMouseDown
    void OnMouseDown()
    {
        float distance = Vector3.Distance(PlayerStats.instance.transform.position, transform.position);
        if (distance <= interactionDistance)
        {
            switch (type)
            {
                case NPCType.Shopkeeper:
                    // Art�k do�rudan kendi �zerindeki ShopData'y� g�nderiyor
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
            Debug.Log("NPC'den �ok uzaktas�n!");
        }
    }
    #endregion
}