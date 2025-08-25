// Inventory.cs (MATERYAL KONTROLÜ VE HARCAMA EKLENDÝ)
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // LINQ kütüphanesini ekliyoruz, sayma iþlemi için çok kullanýþlý.

public class Inventory : MonoBehaviour
{
    #region Singleton
    public static Inventory instance;
    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
    }
    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 20;
    public List<Item> items = new List<Item>();

    public bool Add(Item item)
    {
        // ... (Bu fonksiyon ayný kalýyor) ...
        #region Add Fonksiyonu
        if (item == null) return false;
        if (!item.isStackable)
        {
            if (items.Exists(x => x.name == item.name))
            {
                Debug.Log(item.name + " zaten envanterde var ve biriktirilemez. Eklenmedi.");
                return false;
            }
        }
        if (items.Count >= space)
        {
            Debug.Log("Envanter dolu.");
            return false;
        }
        items.Add(item);
        onItemChangedCallback?.Invoke();
        return true;
        #endregion
    }

    public void Remove(Item item)
    {
        // ... (Bu fonksiyon ayný kalýyor) ...
        #region Remove Fonksiyonu
        if (item == null) return;
        items.Remove(item);
        onItemChangedCallback?.Invoke();
        #endregion
    }

    // --- YENÝ FONKSÝYONLAR ---

    // Bir materyal listesinin envanterde olup olmadýðýný kontrol eder.
    public bool HasMaterials(List<MaterialCost> costs)
    {
        foreach (MaterialCost cost in costs)
        {
            // Envanterdeki o materyalin sayýsýný bul
            int amountInInventory = items.Count(x => x.name == cost.material.name);
            // Eðer gereken miktardan daha az varsa, anýnda "yok" de ve çýk.
            if (amountInInventory < cost.quantity)
            {
                Debug.Log("Yeterli " + cost.material.name + " yok. Gereken: " + cost.quantity + ", Mevcut: " + amountInInventory);
                return false;
            }
        }
        // Eðer döngü bittiyse ve hiç "yok" demediysek, demek ki tüm materyaller var.
        return true;
    }

    // Bir materyal listesini envanterden siler.
    public void ConsumeMaterials(List<MaterialCost> costs)
    {
        foreach (MaterialCost cost in costs)
        {
            // Silinmesi gereken her bir materyal için envanterde bir arama yap ve bulduðunu sil.
            for (int i = 0; i < cost.quantity; i++)
            {
                Item itemToRemove = items.Find(x => x.name == cost.material.name);
                if (itemToRemove != null)
                {
                    Remove(itemToRemove);
                }
            }
        }
    }
}