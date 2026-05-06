using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FixedAspect : MonoBehaviour
{
    public Vector2 targetAspect = new Vector2(16, 9);

    void Start()
    {
        ApplyLetterbox();
    }

    void OnEnable()
    {
        ApplyLetterbox();
    }

    void Update()
    {
        // Garantiza que cambie incluso si Unity reajusta el Game View
        ApplyLetterbox();
    }

    void ApplyLetterbox()
    {
        Camera cam = GetComponent<Camera>();

        float target = targetAspect.x / targetAspect.y;
        float window = (float)Screen.width / Screen.height;

        if (Mathf.Approximately(window, target))
        {
            cam.rect = new Rect(0, 0, 1, 1);
            return;
        }

        if (window < target)
        {
            float scale = window / target;
            cam.rect = new Rect(0, (1f - scale) / 2f, 1f, scale);
        }
        else
        {
            float scale = target / window;
            cam.rect = new Rect((1f - scale) / 2f, 0, scale, 1f);
        }
    }
}
