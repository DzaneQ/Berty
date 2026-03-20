using Berty.Enums;
using Berty.Grid.Entities;
using Berty.Settings;
using Berty.Utility;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Berty.Gameplay.Init
{
    public class LanguageInit : MonoBehaviour
    {
        public void UpdateLanguageForFixedLabels()
        {
            Dictionary<string, string> dict = LoadLanguageDictionary();
            TMP_Text[] labels = GetTranslatableLabels();
            TranslateLabels(labels, dict);
        }

        private Dictionary<string, string> LoadLanguageDictionary()
        {
            string languageFileName = GetFileNameFromLanguage(SettingsManager.Instance.Language);
            TextAsset textAsset = Resources.Load<TextAsset>($"Translation/init/{languageFileName}");
            if (textAsset == null) throw new Exception($"Undefined file of name: /misc/{languageFileName}.");
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(textAsset.text);
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

        private TMP_Text[] GetTranslatableLabels()
        {
            // Get all text belonging to objects which are the first child of its parent.
            return FindObjectsOfType<TMP_Text>(true).Where(text => text.transform.parent.GetChild(0) == text.transform).ToArray();
        }

        private void TranslateLabels(TMP_Text[] labels, Dictionary<string, string> dict)
        {
            foreach (TMP_Text label in labels)
            {
                string key = label.transform.parent.name;
                if (!dict.ContainsKey(key)) continue;
                label.text = dict[key];
            }
        }
    }
}
