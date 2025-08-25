// UIClickDebugger.cs (YENÝ SCRÝPT)
// Týklamanýn altýndaki TÜM UI objelerini listeler.
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIClickDebugger : MonoBehaviour
{
    void Update()
    {
        // Fare sol veya sað tuþuna basýldýðýnda
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            // EventSystem'den anlýk fare verisini al
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            // Raycast sonuçlarýný saklamak için bir liste oluþtur
            List<RaycastResult> results = new List<RaycastResult>();

            // Raycast'i çalýþtýr ve fare altýndaki tüm objeleri listeye ekle
            EventSystem.current.RaycastAll(pointerData, results);

            // Eðer en az bir obje bulunduysa...
            if (results.Count > 0)
            {
                Debug.Log("--- Týklama Algýlandý! Fare altýndaki objeler (en üstten en alta): ---");

                // Listeyi konsola yazdýr
                foreach (var result in results)
                {
                    // Týklanan objenin adýný ve katmanýný yazdýr. Objenin kendisini de log'a ekleyerek
                    // üzerine týklandýðýnda Hierarchy'de seçilmesini saðla.
                    Debug.Log("Obje: " + result.gameObject.name, result.gameObject);
                }
                Debug.Log("---------------------------------------------------------");
            }
        }
    }
}
