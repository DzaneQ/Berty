using Berty.Enums;
using Berty.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguagePickMenu : MonoBehaviour
{
    public void ChangeLanguageTo(string languageStr)
    {
        Debug.Log("Changing language to: " + languageStr);
        LanguageEnum language = languageStr switch
        {
            "pl" => LanguageEnum.Polish,
            "en" => LanguageEnum.English,
            _ => LanguageEnum.English
        };
        SettingsManager.Instance.SetLanguage(language);
    }
}
