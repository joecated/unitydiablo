// SummonSkill.cs (YENÝ SCRÝPT)
using UnityEngine;

[CreateAssetMenu(fileName = "New Summon Skill", menuName = "Skills/Summon Skill")]
public class SummonSkill : Skill
{
    [Header("Çaðýrma Ayarlarý")]
    public GameObject summonPrefab; // Çaðýracaðýmýz yaratýðýn prefab'ý (Hydra)
    public float summonDuration = 10f; // Yaratýk ne kadar süre hayatta kalacak

    [Header("Hasar Ayarlarý")]
    // YENÝ: Hydra'nýn vuruþlarý, Yetenek Gücümüzün bu yüzdesi kadar hasar verecek.
    // 0.2 = %20, 1.0 = %100
    public float abilityPowerScaling = 0.9f;

    public override void Activate(PlayerCombat combatController)
    {
        // Bu yetenek türü için özel fonksiyonu PlayerCombat'ta çaðýr
        combatController.DoSummonSkill(this);
    }
}