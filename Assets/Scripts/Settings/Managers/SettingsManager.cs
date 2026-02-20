using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Settings
{
    public class SettingsManager : PersistentManagerSingleton<SettingsManager>
    {
        public float Volume { get; private set; } = 1f;
        public LanguageEnum Language { get; private set; } = LanguageEnum.Polish;

        public void SetVolume(float value)
        {
            Volume = value;
            EventManager.Instance.RaiseOnVolumeChanged();
        }

        public void SetLanguage(LanguageEnum language)
        {
            Language = language;
            // TODO: Change language in menu
        }
    }
}
