using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Gameplay.Managers
{
    public class LanguageManager : ManagerSingleton<LanguageManager>
    {
        private Dictionary<string, string> languageDictionary;

        private void Awake()
        {
            InitializeSingleton();
            LoadLanguageDictionary();
        }

        public string GetTextFromKey(string key)
        {
            if (!languageDictionary.ContainsKey(key)) throw new KeyNotFoundException($"Key '{key}' not found in languageDictionary.");
            return languageDictionary[key];
        }

        private void LoadLanguageDictionary()
        {
            string languageFileName = GetFileNameFromLanguage(CoreManager.Instance.Game.GameConfig.Language);
            TextAsset textAsset = Resources.Load<TextAsset>($"Translation/{languageFileName}");
            if (textAsset == null) throw new Exception($"Undefined file of name: {languageFileName}.");
            languageDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text);
        }

        private string GetFileNameFromLanguage(Language language)
        {
            return language switch
            {
                Language.Polish => "pl",
                Language.English => "en",
                _ => throw new ArgumentException("Undefined language name."),
            };
        }
    }
}
