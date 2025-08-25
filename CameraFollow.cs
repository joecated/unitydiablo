// CameraFollow.cs (DAHA BASÝT VE NET VERSÝYON)
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;

    [Tooltip("Kameranýn oyuncudan ne kadar uzakta duracaðý.")]
    public Vector3 offset; // ARTIK PUBLIC! Inspector'dan biz ayarlayacaðýz.

    [Tooltip("Takibin ne kadar yumuþak olacaðý. Düþük deðerler daha yumuþak.")]
    [Range(0.01f, 1.0f)]
    public float smoothSpeed = 0.125f;

    void Start()
    {
        // Oyuncuyu her zamanki gibi otomatik bul
        if (PlayerStats.instance != null)
        {
            target = PlayerStats.instance.transform;
        }
        else
        {
            Debug.LogError("Kamera takip edecek bir oyuncu bulamadý!");
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Olmasý gereken pozisyonu, oyuncunun pozisyonu + bizim belirlediðimiz mesafe olarak hesapla
            Vector3 desiredPosition = target.position + offset;

            // Mevcut pozisyondan olmasý gereken pozisyona doðru yumuþak bir geçiþ yap
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Kameranýn pozisyonunu güncelle
            transform.position = smoothedPosition;
        }
    }
}