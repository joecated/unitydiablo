// SummonSkill.cs (YEN� SCR�PT)
using UnityEngine;

[CreateAssetMenu(fileName = "New Summon Skill", menuName = "Skills/Summon Skill")]
public class SummonSkill : Skill
{
    [Header("�a��rma Ayarlar�")]
    public GameObject summonPrefab; // �a��raca��m�z yarat���n prefab'� (Hydra)
    public float summonDuration = 10f; // Yarat�k ne kadar s�re hayatta kalacak

    [Header("Hasar Ayarlar�")]
    // YEN�: Hydra'n�n vuru�lar�, Yetenek G�c�m�z�n bu y�zdesi kadar hasar verecek.
    // 0.2 = %20, 1.0 = %100
    public float abilityPowerScaling = 0.9f;

    public override void Activate(PlayerCombat combatController)
    {
        // Bu yetenek t�r� i�in �zel fonksiyonu PlayerCombat'ta �a��r
        combatController.DoSummonSkill(this);
    }
}