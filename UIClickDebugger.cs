// UIClickDebugger.cs (YEN� SCR�PT)
// T�klaman�n alt�ndaki T�M UI objelerini listeler.
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIClickDebugger : MonoBehaviour
{
    void Update()
    {
        // Fare sol veya sa� tu�una bas�ld���nda
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            // EventSystem'den anl�k fare verisini al
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;

            // Raycast sonu�lar�n� saklamak i�in bir liste olu�tur
            List<RaycastResult> results = new List<RaycastResult>();

            // Raycast'i �al��t�r ve fare alt�ndaki t�m objeleri listeye ekle
            EventSystem.current.RaycastAll(pointerData, results);

            // E�er en az bir obje bulunduysa...
            if (results.Count > 0)
            {
                Debug.Log("--- T�klama Alg�land�! Fare alt�ndaki objeler (en �stten en alta): ---");

                // Listeyi konsola yazd�r
                foreach (var result in results)
                {
                    // T�klanan objenin ad�n� ve katman�n� yazd�r. Objenin kendisini de log'a ekleyerek
                    // �zerine t�kland���nda Hierarchy'de se�ilmesini sa�la.
                    Debug.Log("Obje: " + result.gameObject.name, result.gameObject);
                }
                Debug.Log("---------------------------------------------------------");
            }
        }
    }
}
