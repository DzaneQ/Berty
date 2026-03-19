using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Settings;
using Berty.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings.Switch;

namespace Berty.Menu.Managers
{
    public class MenuLanguageManager : ManagerSingleton<MenuLanguageManager>
    {
        private TMP_Text[] _labels;
        private Dictionary<(string key, LanguageEnum language), string> languageDictionaries;

        protected override void Awake()
        {
            base.Awake();
            InstantiateTranslatableLabels();
            LoadLanguageDictionaries();
        }

        public void RefreshLabels()
        {
            LanguageEnum currentLanguage = SettingsManager.Instance.Language;
            foreach (TMP_Text label in _labels)
            {
                string key = label.transform.parent.name;
                label.text = languageDictionaries[(key, currentLanguage)];
            }
        }

        private void InstantiateTranslatableLabels()
        {
            // Get all text belonging to objects which are the first child of its parent.
            _labels = FindObjectsOfType<TMP_Text>(true).Where(text => text.transform.parent.GetChild(0) == text.transform).ToArray();
        }

        private void LoadLanguageDictionaries()
        {
            languageDictionaries = new();
            foreach (LanguageEnum language in GetAllLanguages())
            {
                string languageFileName = GetFileNameFromLanguage(language);
                TextAsset textAsset = Resources.Load<TextAsset>($"Translation/menu/{languageFileName}");
                if (textAsset == null) throw new Exception($"Undefined file of name: /menu/{languageFileName}.");
                Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text);
                foreach (KeyValuePair<string, string> kvp in dict)
                {
                    languageDictionaries[(kvp.Key, language)] = kvp.Value;
                }    
            }
        }

        private LanguageEnum[] GetAllLanguages()
        {
            return (LanguageEnum[])Enum.GetValues(typeof(LanguageEnum));
        }

        private string GetFileNameFromLanguage(LanguageEnum language)
        {
            return language switch
            {
                LanguageEnum.Polish => "pl",
                LanguageEnum.English => "en",
                _ => throw new ArgumentException("Undefined language name."),
            };
        }
    }
}