// TooltipTrigger.cs (YENÝ YAPI ÝÇÝN GÜNCELLENDÝ)
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;
    [TextArea(3, 10)]
    public string content;
    public string manaCost;
    public string cooldown;

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipSystem.Show(content, header, manaCost, cooldown);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipSystem.Hide();
    }
}