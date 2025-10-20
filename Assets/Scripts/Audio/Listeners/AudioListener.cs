using Berty.BoardCards.Behaviours;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Settings;
using System;
using UnityEngine;

namespace Berty.BoardCards.Listeners
{
    public class AudioListener : MonoBehaviour
    {
        private AudioSource soundSource;

        private void Awake()
        {
            soundSource = GetComponent<AudioSource>();
            if (soundSource == null) throw new Exception($"Unable to get audio source for {name}");
        }

        private void OnEnable()
        {
            EventManager.Instance.OnVolumeChanged += HandleVolumeChanged;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnVolumeChanged -= HandleVolumeChanged;
        }

        private void HandleVolumeChanged()
        {
            soundSource.volume = SettingsManager.Instance.Volume;
        }
    }
}