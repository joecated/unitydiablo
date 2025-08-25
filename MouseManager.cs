// MouseManager.cs (N�HA� VE EN SA�LAM HAL�)
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private EnemyHighlighter lastHighlightedEnemy;
    // Art�k private bir kamera de�i�kenine bile ihtiyac�m�z yok.

    void Update()
    {
        // E�er CameraProvider hen�z kameras�n� global adrese kaydetmediyse, bekle.
        if (CameraProvider.Main == null)
        {
            // Bu kontrol sayesinde oyunun ilk frame'inde hata almay�z.
            return;
        }

        // Do�rudan o %100 g�venilir referans� kullan!
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