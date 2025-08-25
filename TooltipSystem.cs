// TooltipSystem.cs (YENÝ YAPI ÝÇÝN GÜNCELLENDÝ)
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    public static TooltipSystem instance;
    public Tooltip tooltip;

    void Awake()
    {
        instance = this;
    }

    public static void Show(string content, string header = "", string manaCost = "", string cooldown = "")
    {
        instance.tooltip.gameObject.SetActive(true);
        instance.tooltip.SetText(header, content, manaCost, cooldown);
        instance.tooltip.Show();
    }

    public static void Hide()
    {
        if (instance != null && instance.tooltip != null)
        {
            instance.tooltip.Hide();
            instance.tooltip.gameObject.SetActive(false);
        }
    }
}