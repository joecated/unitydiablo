using UnityEngine;
public class BlacksmithNPC : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("Demirciye t�kland�!");
        BlacksmithUI.instance.OpenBlacksmith();
    }
}