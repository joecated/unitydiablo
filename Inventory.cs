// Inventory.cs (MATERYAL KONTROL� VE HARCAMA EKLEND�)
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // LINQ k�t�phanesini ekliyoruz, sayma i�lemi i�in �ok kullan��l�.

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
        // ... (Bu fonksiyon ayn� kal�yor) ...
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
        // ... (Bu fonksiyon ayn� kal�yor) ...
        #region Remove Fonksiyonu
        if (item == null) return;
        items.Remove(item);
        onItemChangedCallback?.Invoke();
        #endregion
    }

    // --- YEN� FONKS�YONLAR ---

    // Bir materyal listesinin envanterde olup olmad���n� kontrol eder.
    public bool HasMaterials(List<MaterialCost> costs)
    {
        foreach (MaterialCost cost in costs)
        {
            // Envanterdeki o materyalin say�s�n� bul
            int amountInInventory = items.Count(x => x.name == cost.material.name);
            // E�er gereken miktardan daha az varsa, an�nda "yok" de ve ��k.
            if (amountInInventory < cost.quantity)
            {
                Debug.Log("Yeterli " + cost.material.name + " yok. Gereken: " + cost.quantity + ", Mevcut: " + amountInInventory);
                return false;
            }
        }
        // E�er d�ng� bittiyse ve hi� "yok" demediysek, demek ki t�m materyaller var.
        return true;
    }

    // Bir materyal listesini envanterden siler.
    public void ConsumeMaterials(List<MaterialCost> costs)
    {
        foreach (MaterialCost cost in costs)
        {
            // Silinmesi gereken her bir materyal i�in envanterde bir arama yap ve buldu�unu sil.
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