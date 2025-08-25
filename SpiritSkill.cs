// SpiritSkill.cs (YENÝ SCRÝPT)
using UnityEngine;

[CreateAssetMenu(fileName = "New Spirit Skill", menuName = "Skills/Spirit Skill")]
public class SpiritSkill : Skill
{
    [Header("Ruh Formu Ayarlarý")]
    public float duration = 5f;
    public float dashDistance = 5f;
    public GameObject bodyPrefab; // Geride býrakýlacak beden prefab'ý
    public GameObject tetherPrefab; // YENÝ: Beden ile ruh arasýndaki bað prefab'ý
    public GameObject spiritVFX; // Ruh formundayken karakterde görünecek efekt

    [Header("Hasar Yansýmasý (%)")]
    public float minDamageReturnPercent = 15f;
    public float maxDamageReturnPercent = 95f;

    [Header("Arayüz")]
    public Skill recastSkillIcon; // E'ye tekrar basmak için görünecek ikon

    public override void Activate(PlayerCombat combatController)
    {
        combatController.DoSpiritSkill(this);
    }
}