using UnityEngine;

public class BlackBars : MonoBehaviour
{
    public float targetAspectWidth = 16f;
    public float targetAspectHeight = 9f;

    void Start()
    {
        float targetAspect = targetAspectWidth / targetAspectHeight;
        float windowAspect = (float)Screen.width / (float)Screen.height;

        float scaleHeight = windowAspect / targetAspect;

        Camera cam = Camera.main;

        if (scaleHeight < 1f)
        {
            // Pantalla más alta ? barras negras ARRIBA y ABAJO
            Rect rect = cam.rect;

            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0f;
            rect.y = (1f - scaleHeight) / 2f;

            cam.rect = rect;
        }
        else
        {
            // Pantalla más ancha ? barras negras a los lados
            float scaleWidth = 1f / scaleHeight;

            Rect rect = cam.rect;

            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f;
            rect.y = 0f;

            cam.rect = rect;
        }
    }
}
