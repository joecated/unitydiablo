using UnityEngine;
public class BlacksmithNPC : MonoBehaviour
{
    private void OnMouseDown()
    {
        Debug.Log("Demirciye týklandý!");
        BlacksmithUI.instance.OpenBlacksmith();
    }
}