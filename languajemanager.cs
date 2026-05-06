using UnityEngine;
using System.Collections.Generic;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance { get; private set; }

    public enum Language { Spanish = 0, English = 1 }
    public Language currentLanguage = Language.Spanish;

    private Dictionary<string, string> spanishTexts;
    private Dictionary<string, string> englishTexts;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        InitializeTexts();
        LoadLanguage();
    }

    void InitializeTexts()
    {
        spanishTexts = new Dictionary<string, string>()
        {
            {"start", "Iniciar"},
            {"options", "Opciones"},
            {"exit", "Salir"},
            {"language", "Idioma"},
            {"spanish", "Español"},
            {"english", "Inglés"}
        };

        englishTexts = new Dictionary<string, string>()
        {
            {"start", "Start"},
            {"options", "Options"},
            {"exit", "Exit"},
            {"language", "Language"},
            {"spanish", "Spanish"},
            {"english", "English"}
        };
    }

    public void SetLanguage(Language lang)
    {
        currentLanguage = lang;
        PlayerPrefs.SetInt("Language", (int)lang);
        PlayerPrefs.Save();
        RefreshAllLanguageTexts();
    }

    public void LoadLanguage()
    {
        int saved = PlayerPrefs.GetInt("Language", (int)Language.Spanish);
        if (saved >= 0 && saved <= 1)
            currentLanguage = (Language)saved;
        else
            currentLanguage = Language.Spanish;
    }

    public string GetText(string key)
    {
        if (string.IsNullOrEmpty(key)) return "";

        switch (currentLanguage)
        {
            case Language.English:
                if (englishTexts != null && englishTexts.ContainsKey(key)) return englishTexts[key];
                break;
            default:
                if (spanishTexts != null && spanishTexts.ContainsKey(key)) return spanishTexts[key];
                break;
        }

        return key;
    }

    void RefreshAllLanguageTexts()
    {
        LanguageText[] texts = FindObjectsOfType<LanguageText>(true);
        foreach (var t in texts)
            t.UpdateText();
    }
}
