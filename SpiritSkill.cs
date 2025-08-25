// SpiritSkill.cs (YEN� SCR�PT)
using UnityEngine;

[CreateAssetMenu(fileName = "New Spirit Skill", menuName = "Skills/Spirit Skill")]
public class SpiritSkill : Skill
{
    [Header("Ruh Formu Ayarlar�")]
    public float duration = 5f;
    public float dashDistance = 5f;
    public GameObject bodyPrefab; // Geride b�rak�lacak beden prefab'�
    public GameObject tetherPrefab; // YEN�: Beden ile ruh aras�ndaki ba� prefab'�
    public GameObject spiritVFX; // Ruh formundayken karakterde g�r�necek efekt

    [Header("Hasar Yans�mas� (%)")]
    public float minDamageReturnPercent = 15f;
    public float maxDamageReturnPercent = 95f;

    [Header("Aray�z")]
    public Skill recastSkillIcon; // E'ye tekrar basmak i�in g�r�necek ikon

    public override void Activate(PlayerCombat combatController)
    {
        combatController.DoSpiritSkill(this);
    }
}