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

        public void PlayNewCardSound()
        {
            SoundManager.Instance.PutSound(Source);
        }
    }
}