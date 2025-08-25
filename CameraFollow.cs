// CameraFollow.cs (DAHA BAS�T VE NET VERS�YON)
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;

    [Tooltip("Kameran�n oyuncudan ne kadar uzakta duraca��.")]
    public Vector3 offset; // ARTIK PUBLIC! Inspector'dan biz ayarlayaca��z.

    [Tooltip("Takibin ne kadar yumu�ak olaca��. D���k de�erler daha yumu�ak.")]
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
            Debug.LogError("Kamera takip edecek bir oyuncu bulamad�!");
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Olmas� gereken pozisyonu, oyuncunun pozisyonu + bizim belirledi�imiz mesafe olarak hesapla
            Vector3 desiredPosition = target.position + offset;

            // Mevcut pozisyondan olmas� gereken pozisyona do�ru yumu�ak bir ge�i� yap
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Kameran�n pozisyonunu g�ncelle
            transform.position = smoothedPosition;
        }
    }
}