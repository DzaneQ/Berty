using Berty.Audio.Managers;
using Berty.Settings;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardSound : MonoBehaviour
    {
        private AudioSource soundSource;
        public AudioSource Source => soundSource;

        private void Awake()
        {
            soundSource = GetComponent<AudioSource>();
            soundSource.volume = SettingsManager.Instance.Volume;
        }

        private void Start()
        {
            SoundManager.Instance.PutSound(Source);
        }
    }
}