// ComboSkill.cs (YENÝ SCRÝPT)
// Riven'in Q'su gibi çoklu basýþ gerektiren yetenekleri yönetir.
using UnityEngine;

// [System.Serializable] satýrý, bu class'ýn Inspector'da görünebilmesini saðlar.
[System.Serializable]
// public class ComboStep satýrý, bir kombonun tek bir adýmýnýn özelliklerini (hasar, animasyon vb.) bir arada tutan bir veri yapýsý oluþturur.
public class ComboStep
{
    // public string animationTrigger... satýrý, bu kombo adýmýnda tetiklenecek animasyonun adýný tutar.
    public string animationTrigger = "Attack";
    // public int damage... satýrý, bu adýmýn vereceði hasarý belirler.
    public int damage = 10;
    // public float dashForce... satýrý, bu adýmda karakterin ne kadar ileri atýlacaðýný belirler.
    public float dashForce = 5f;
    public float damageRadius = 2f; // Her vuruþun etki alaný yarýçapý
}

// [CreateAssetMenu...] satýrý, Unity'nin "Create" menüsüne yeni bir "Combo Skill" oluþturma seçeneði ekler.
[CreateAssetMenu(fileName = "New Combo Skill", menuName = "Skills/Combo Skill")]
// public class ComboSkill : Skill satýrý, bu script'in bir "Skill" olduðunu ama kendine özel ek özellikleri olduðunu belirtir.
public class ComboSkill : Skill
{
    // public float comboResetTime... satýrý, bir sonraki kombo vuruþu için ne kadar süremiz olduðunu belirler.
    public float comboResetTime = 2f;
    // public ComboStep[] comboSteps... satýrý, bu yeteneðin tüm kombo adýmlarýný bir dizi içinde tutar.
    public ComboStep[] comboSteps;

    // public override void Activate... satýrý, temel 'Skill' script'indeki 'Activate' fonksiyonunu ezer ve kendi özel mantýðýný çalýþtýrýr.
    public override void Activate(PlayerCombat combatController)
    {
        // combatController.DoComboSkill(this); satýrý, bu yetenek kullanýldýðýnda, 'PlayerCombat' script'indeki kombo yapma fonksiyonunu çaðýrýr ve ona kendi bilgilerini (bu script'i) gönderir.
        combatController.DoComboSkill(this);
    }
}