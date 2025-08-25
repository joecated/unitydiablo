using UnityEngine;

// Bu script, ana kameranýn referansýný oyunun her yerinden eriþilebilir hale getirecek.
public class CameraProvider : MonoBehaviour
{
    // "static" sayesinde bu deðiþkene her yerden "CameraProvider.Main" yazarak ulaþabiliriz.
    public static Camera Main { get; private set; }

    void Awake()
    {
        // Bu script'in baðlý olduðu kamera objesini al ve Main deðiþkenine ata.
        Main = GetComponent<Camera>();
        Debug.Log("<color=cyan>CameraProvider: Ana Kamera referansý ayarlandý!</color>");
    }
}