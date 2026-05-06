using UnityEngine;

public class SelectorFollow : MonoBehaviour
{
    public RectTransform targetButton;  // El botón
    public float offsetY = 10000f;         // Qué tan arriba se coloca la flecha

    void LateUpdate()
    {
        if (targetButton == null) return;

        // Coloca el selector exactamente arriba del botón
        Vector3 newPos = targetButton.position;
        newPos.y += offsetY;

        transform.position = newPos;
    }
}
