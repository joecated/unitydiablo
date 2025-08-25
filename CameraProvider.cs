using UnityEngine;

// Bu script, ana kameran�n referans�n� oyunun her yerinden eri�ilebilir hale getirecek.
public class CameraProvider : MonoBehaviour
{
    // "static" sayesinde bu de�i�kene her yerden "CameraProvider.Main" yazarak ula�abiliriz.
    public static Camera Main { get; private set; }

    void Awake()
    {
        // Bu script'in ba�l� oldu�u kamera objesini al ve Main de�i�kenine ata.
        Main = GetComponent<Camera>();
        Debug.Log("<color=cyan>CameraProvider: Ana Kamera referans� ayarland�!</color>");
    }
}