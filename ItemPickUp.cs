// ItemPickup.cs (YEN� SCR�PT)
// Bu script, yerde duran ve toplanabilen e�ya objelerinin �zerinde olacak.
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item; // Bu obje hangi e�yay� temsil ediyor?

    void OnMouseDown()
    {
        PickUp();
    }

    void PickUp()
    {
        Debug.Log(item.name + " topland�.");
        // Inventory'e bu e�yay� eklemeyi dene.
        bool wasPickedUp = Inventory.instance.Add(item);

        // E�er envantere ba�ar�yla eklendiyse (yer varsa), objeyi yok et.
        if (wasPickedUp)
        {
            Destroy(gameObject);
        }
    }
}