using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public void SetSpanish()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.SetLanguage(LanguageManager.Language.Spanish);
    }

    public void SetEnglish()
    {
        if (LanguageManager.Instance != null)
            LanguageManager.Instance.SetLanguage(LanguageManager.Language.English);
    }
}
