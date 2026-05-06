using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LanguageText : MonoBehaviour
{
    [Tooltip("Clave que usa LanguageManager para buscar el texto")]
    public string key;

    private TextMeshProUGUI textComponent;

    void Awake()
    {
        textComponent = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        if (LanguageManager.Instance != null && !string.IsNullOrEmpty(key))
            textComponent.text = LanguageManager.Instance.GetText(key);
    }
}
