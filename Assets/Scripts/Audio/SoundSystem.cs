using UnityEngine;

namespace Berty.Audio
{
    public class SoundSystem : MonoBehaviour
    {
        private Transform mainTransform;
        private AudioSource soundSrc;

        [SerializeField] private AudioClip moveClip;
        //[SerializeField] private AudioClip attackClip;
        [SerializeField] private AudioClip putCardOnFieldClip;
        [SerializeField] private AudioClip retractCardClip;
        [SerializeField] private AudioClip confirmCardClip;
        [SerializeField] private AudioClip buttonHoverClip;
        [SerializeField] private AudioClip buttonClickClip;
        [SerializeField] private AudioClip cardSelectClip;
        [SerializeField] private AudioClip cardDeselectClip;

        private void Start()
        {
            soundSrc = GetComponent<AudioSource>();
            mainTransform = FindObjectOfType<Camera>().GetComponent<Transform>();
        }

        public void MoveSound(AudioSource src)
        {
            if (src.isPlaying) return;
            src.clip = moveClip;
            src.time = 0f;
            src.Play();
        }

        public void PutSound(AudioSource src)
        {
            src.clip = putCardOnFieldClip;
            src.time = 0.1f;
            src.Play();
        }

        public void TakeSound(Transform src)
        {
            transform.position = src.position;
            soundSrc.clip = retractCardClip;
            soundSrc.time = 0f;
            soundSrc.Play();
        }

        public void AttackSound(AudioSource src, AudioClip clip)
        {
            if (clip == null) return;
            src.clip = clip;
            src.time = 0f;
            src.Play();
        }

        public void ConfirmSound(AudioSource src)
        {
            if (src.isPlaying) return;
            src.clip = confirmCardClip;
            src.time = 0f;
            src.Play();
        }

        public void ButtonClickSound()
        {
            transform.position = mainTransform.position;
            soundSrc.clip = buttonClickClip;
            soundSrc.time = 0f;
            soundSrc.Play();
        }

        public void SelectSound(bool selecting)
        {
            //Debug.Log("Playing cardImage sound during selecting: " + selecting);
            transform.position = mainTransform.position;
            //soundSrc.clip = selecting ? cardSelect : cardDeselect;
            soundSrc.time = 0f;
            //soundSrc.Play();
            soundSrc.PlayOneShot(selecting ? cardSelectClip : cardDeselectClip);
        }
    }
}