// ComboSkill.cs (YEN� SCR�PT)
// Riven'in Q'su gibi �oklu bas�� gerektiren yetenekleri y�netir.
using UnityEngine;

// [System.Serializable] sat�r�, bu class'�n Inspector'da g�r�nebilmesini sa�lar.
[System.Serializable]
// public class ComboStep sat�r�, bir kombonun tek bir ad�m�n�n �zelliklerini (hasar, animasyon vb.) bir arada tutan bir veri yap�s� olu�turur.
public class ComboStep
{
    // public string animationTrigger... sat�r�, bu kombo ad�m�nda tetiklenecek animasyonun ad�n� tutar.
    public string animationTrigger = "Attack";
    // public int damage... sat�r�, bu ad�m�n verece�i hasar� belirler.
    public int damage = 10;
    // public float dashForce... sat�r�, bu ad�mda karakterin ne kadar ileri at�laca��n� belirler.
    public float dashForce = 5f;
    public float damageRadius = 2f; // Her vuru�un etki alan� yar��ap�
}

// [CreateAssetMenu...] sat�r�, Unity'nin "Create" men�s�ne yeni bir "Combo Skill" olu�turma se�ene�i ekler.
[CreateAssetMenu(fileName = "New Combo Skill", menuName = "Skills/Combo Skill")]
// public class ComboSkill : Skill sat�r�, bu script'in bir "Skill" oldu�unu ama kendine �zel ek �zellikleri oldu�unu belirtir.
public class ComboSkill : Skill
{
    // public float comboResetTime... sat�r�, bir sonraki kombo vuru�u i�in ne kadar s�remiz oldu�unu belirler.
    public float comboResetTime = 2f;
    // public ComboStep[] comboSteps... sat�r�, bu yetene�in t�m kombo ad�mlar�n� bir dizi i�inde tutar.
    public ComboStep[] comboSteps;

    // public override void Activate... sat�r�, temel 'Skill' script'indeki 'Activate' fonksiyonunu ezer ve kendi �zel mant���n� �al��t�r�r.
    public override void Activate(PlayerCombat combatController)
    {
        // combatController.DoComboSkill(this); sat�r�, bu yetenek kullan�ld���nda, 'PlayerCombat' script'indeki kombo yapma fonksiyonunu �a��r�r ve ona kendi bilgilerini (bu script'i) g�nderir.
        combatController.DoComboSkill(this);
    }
}