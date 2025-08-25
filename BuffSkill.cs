// BuffSkill.cs (YEN� SCR�PT)
// Riven'in R'si gibi karaktere ge�ici g��lendirmeler veren yetenekleri y�netir.
using UnityEngine;

// [CreateAssetMenu...] sat�r�, Unity'nin "Create" men�s�ne yeni bir "Buff Skill" olu�turma se�ene�i ekler.
[CreateAssetMenu(fileName = "New Buff Skill", menuName = "Skills/Buff Skill")]
// public class BuffSkill : Skill sat�r�, bu script'in bir "Skill" oldu�unu belirtir.
public class BuffSkill : Skill
{
    // public float duration... sat�r�, g��lendirmenin ne kadar s�rece�ini belirler.
    public float duration = 15f;
    // public int damageBonus... sat�r�, bu g��lendirmenin ne kadar ekstra sald�r� g�c� verece�ini belirler.
    public int damageBonus = 20;
    // public Skill secondCastSkill... sat�r�, bu yetenek aktifken kullan�labilecek ikinci yetene�i (Wind Slash) tutar.
    public Skill secondCastSkill;

    // public override void Activate... sat�r�, temel 'Skill' script'indeki 'Activate' fonksiyonunu ezer.
    public override void Activate(PlayerCombat combatController)
    {
        // combatController.DoBuffSkill(this); sat�r�, bu yetenek kullan�ld���nda, 'PlayerCombat' script'indeki g��lendirme fonksiyonunu �a��r�r ve ona kendi bilgilerini g�nderir.
        combatController.DoBuffSkill(this);
    }
}