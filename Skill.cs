// Skill.cs (GÜNCELLENDÝ - Menzil ve VFX Referansý Eklendi)
// Bu, tüm yeteneklerimizin temelini oluþturan ana script'tir.
using UnityEngine;

public enum SkillType { Damage, Heal, Combo, Buff,  Projectile, Summon, Spirit }

[CreateAssetMenu(fileName = "New Skill", menuName = "Skills/Skill")]
public class Skill : ScriptableObject
{
    new public string name = "New Skill";
    public Sprite icon = null;
    [TextArea(3, 10)]
    public string description = "Yetenek açýklamasý buraya gelecek.";

    public SkillType skillType = SkillType.Damage;
    public int damage = 0;
    public DamageType damageType = DamageType.Physical; // YENÝ: Yeteneðin hasar türü
    public float apScaling = 0f; // YENÝ: Yeteneðin AP'den ne kadar güçlendiði (örn: 0.8 = AP'nin %80'i)


    public int healAmount = 0;
    public int manaCost = 5;
    public float cooldown = 1f;
    public float range = 3f; // HATA DÜZELTMESÝ: Eksik olan menzil deðiþkeni
    public GameObject skillVFX; // Heal efekti için

    // Bir yetenek kullanýldýðýnda çalýþacak olan ana fonksiyon.
    public virtual void Activate(PlayerCombat combatController)
    {
        // Basit hasar ve iyileþtirme yetenekleri için varsayýlan davranýþ
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