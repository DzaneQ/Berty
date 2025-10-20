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

        public void SetVolume(float value)
        {
            //Debug.Log($"Setting volume from {Volume} to {value}");
            Volume = value;
            EventManager.Instance.RaiseOnVolumeChanged();
        }
    }
}
