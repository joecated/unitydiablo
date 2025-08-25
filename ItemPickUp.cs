// ItemPickup.cs (YENÝ SCRÝPT)
// Bu script, yerde duran ve toplanabilen eþya objelerinin üzerinde olacak.
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item; // Bu obje hangi eþyayý temsil ediyor?

    void OnMouseDown()
    {
        PickUp();
    }

    void PickUp()
    {
        Debug.Log(item.name + " toplandý.");
        // Inventory'e bu eþyayý eklemeyi dene.
        bool wasPickedUp = Inventory.instance.Add(item);

        // Eðer envantere baþarýyla eklendiyse (yer varsa), objeyi yok et.
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
    }
}