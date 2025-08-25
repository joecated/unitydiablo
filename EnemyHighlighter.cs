// EnemyHighlighter.cs (YENÝ SCRÝPT)
// Her düþmanýn üzerine eklenecek ve kendi vurgu objesini kontrol edecek.
using UnityEngine;

public class EnemyHighlighter : MonoBehaviour
{
    public GameObject highlightObject; // Inspector'dan atanacak olan halka objesi

    void Start()
    {
        // Oyun baþýnda vurgunun kapalý olduðundan emin ol
        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }
    }

    public void Highlight()
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(true);
        }
    }

    public void UnHighlight()
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(false);
        }
    }
}