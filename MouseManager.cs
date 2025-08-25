// MouseManager.cs (NÝHAÝ VE EN SAÐLAM HALÝ)
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private EnemyHighlighter lastHighlightedEnemy;
    // Artýk private bir kamera deðiþkenine bile ihtiyacýmýz yok.

    void Update()
    {
        // Eðer CameraProvider henüz kamerasýný global adrese kaydetmediyse, bekle.
        if (CameraProvider.Main == null)
        {
            // Bu kontrol sayesinde oyunun ilk frame'inde hata almayýz.
            return;
        }

        // Doðrudan o %100 güvenilir referansý kullan!
        Ray ray = CameraProvider.Main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            EnemyHighlighter highlighter = hit.collider.GetComponent<EnemyHighlighter>();

            if (highlighter != null)
            {
                if (highlighter != lastHighlightedEnemy)
                {
                    if (lastHighlightedEnemy != null)
                    {
                        lastHighlightedEnemy.UnHighlight();
                    }
                    highlighter.Highlight();
                    lastHighlightedEnemy = highlighter;
                }
            }
            else
            {
                if (lastHighlightedEnemy != null)
                {
                    lastHighlightedEnemy.UnHighlight();
                    lastHighlightedEnemy = null;
                }
            }
        }
        else
        {
            if (lastHighlightedEnemy != null)
            {
                lastHighlightedEnemy.UnHighlight();
                lastHighlightedEnemy = null;
            }
        }
    }
}