// Skill.cs (G�NCELLEND� - Menzil ve VFX Referans� Eklendi)
// Bu, t�m yeteneklerimizin temelini olu�turan ana script'tir.
using UnityEngine;

public enum SkillType { Damage, Heal, Combo, Buff,  Projectile, Summon, Spirit }

[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Skill")]
public class Skill : ScriptableObject
{
    new public string name = "New Skill";
    public Sprite icon = null;
    [TextArea(3, 10)]
    public string description = "Yetenek a��klamas� buraya gelecek.";

    public SkillType skillType = SkillType.Damage;
    public int damage = 0;
    public DamageType damageType = DamageType.Physical; // YEN�: Yetene�in hasar t�r�
    public float apScaling = 0f; // YEN�: Yetene�in AP'den ne kadar g��lendi�i (�rn: 0.8 = AP'nin %80'i)


    public int healAmount = 0;
    public int manaCost = 5;
    public float cooldown = 1f;
    public float range = 3f; // HATA D�ZELTMES�: Eksik olan menzil de�i�keni
    public GameObject skillVFX; // Heal efekti i�in

    // Bir yetenek kullan�ld���nda �al��acak olan ana fonksiyon.
    public virtual void Activate(PlayerCombat combatController)
    {
        // Basit hasar ve iyile�tirme yetenekleri i�in varsay�lan davran��
        if (skillType == SkillType.Damage)
        {
            combatController.DoDamageSkill(this);
        }
        else if (skillType == SkillType.Heal)
        {
            combatController.DoHealSkill(this);
        }
        //else if (skillType == SkillType.AreaOfEffect)
        {
           // combatController.DoAreaOfEffectSkill(this);
        }
    }
}