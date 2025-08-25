// BuffSkill.cs (YENÝ SCRÝPT)
// Riven'in R'si gibi karaktere geçici güçlendirmeler veren yetenekleri yönetir.
using UnityEngine;

// [CreateAssetMenu...] satýrý, Unity'nin "Create" menüsüne yeni bir "Buff Skill" oluþturma seçeneði ekler.
[CreateAssetMenu(fileName = "New Buff Skill", menuName = "Skills/Buff Skill")]
// public class BuffSkill : Skill satýrý, bu script'in bir "Skill" olduðunu belirtir.
public class BuffSkill : Skill
{
    // public float duration... satýrý, güçlendirmenin ne kadar süreceðini belirler.
    public float duration = 15f;
    // public int damageBonus... satýrý, bu güçlendirmenin ne kadar ekstra saldýrý gücü vereceðini belirler.
    public int damageBonus = 20;
    // public Skill secondCastSkill... satýrý, bu yetenek aktifken kullanýlabilecek ikinci yeteneði (Wind Slash) tutar.
    public Skill secondCastSkill;

    // public override void Activate... satýrý, temel 'Skill' script'indeki 'Activate' fonksiyonunu ezer.
    public override void Activate(PlayerCombat combatController)
    {
        // combatController.DoBuffSkill(this); satýrý, bu yetenek kullanýldýðýnda, 'PlayerCombat' script'indeki güçlendirme fonksiyonunu çaðýrýr ve ona kendi bilgilerini gönderir.
        combatController.DoBuffSkill(this);
    }
}