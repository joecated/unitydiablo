// ProjectileSkill.cs (YENÝ SCRÝPT)
// Riven'in Wind Slash'i gibi mermi fýrlatan yetenekleri yönetir.
using UnityEngine;

// [CreateAssetMenu...] satýrý, Unity'nin "Create" menüsüne yeni bir "Projectile Skill" oluþturma seçeneði ekler.
[CreateAssetMenu(fileName = "New Projectile Skill", menuName = "Skills/Projectile Skill")]
// public class ProjectileSkill : Skill satýrý, bu script'in bir "Skill" olduðunu belirtir.
public class ProjectileSkill : Skill
{
    // public GameObject projectilePrefab... satýrý, bu yetenek kullanýldýðýnda fýrlatýlacak olan mermi prefab'ýný tutar.
    public GameObject projectilePrefab;
    // public int damage... satýrý, merminin ne kadar hasar vereceðini belirler.
    //public int damage = 50;

    // public override void Activate... satýrý, temel 'Skill' script'indeki 'Activate' fonksiyonunu ezer.
    public override void Activate(PlayerCombat combatController)
    {
        // combatController.DoProjectileSkill(this); satýrý, bu yetenek kullanýldýðýnda, 'PlayerCombat' script'indeki mermi fýrlatma fonksiyonunu çaðýrýr ve ona kendi bilgilerini gönderir.
        combatController.DoProjectileSkill(this);
    }
}