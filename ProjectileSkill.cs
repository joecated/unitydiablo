// ProjectileSkill.cs (YEN� SCR�PT)
// Riven'in Wind Slash'i gibi mermi f�rlatan yetenekleri y�netir.
using UnityEngine;

// [CreateAssetMenu...] sat�r�, Unity'nin "Create" men�s�ne yeni bir "Projectile Skill" olu�turma se�ene�i ekler.
[CreateAssetMenu(fileName = "New Projectile Skill", menuName = "Skills/Projectile Skill")]
// public class ProjectileSkill : Skill sat�r�, bu script'in bir "Skill" oldu�unu belirtir.
public class ProjectileSkill : Skill
{
    // public GameObject projectilePrefab... sat�r�, bu yetenek kullan�ld���nda f�rlat�lacak olan mermi prefab'�n� tutar.
    public GameObject projectilePrefab;
    // public int damage... sat�r�, merminin ne kadar hasar verece�ini belirler.
    //public int damage = 50;

    // public override void Activate... sat�r�, temel 'Skill' script'indeki 'Activate' fonksiyonunu ezer.
    public override void Activate(PlayerCombat combatController)
    {
        // combatController.DoProjectileSkill(this); sat�r�, bu yetenek kullan�ld���nda, 'PlayerCombat' script'indeki mermi f�rlatma fonksiyonunu �a��r�r ve ona kendi bilgilerini g�nderir.
        combatController.DoProjectileSkill(this);
    }
}