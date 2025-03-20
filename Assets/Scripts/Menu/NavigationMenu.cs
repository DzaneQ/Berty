using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Berty.Menu
{
    public class NavigationMenu : MonoBehaviour
    {
        [SerializeField] private GameObject GuideSheet;
        [SerializeField] private GameObject MenuList;
        [SerializeField] private GameObject ExitButton;
        [SerializeField] private AudioClip guideCloseClip;
        private AudioSource src;
        private ButtonFunction playButton;
        private ButtonFunction guideButton;
        private ButtonFunction exitButton;

        private void Start()
        {
            src = GetComponent<AudioSource>();
            playButton = MenuList.transform.GetChild(0).GetComponent<ButtonFunction>();
            guideButton = MenuList.transform.GetChild(1).GetComponent<ButtonFunction>();
            exitButton = ExitButton.GetComponent<ButtonFunction>();

            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                ExitButton.SetActive(true);
            }
        }

        public void Play()
        {
            PlaySound(playButton.ClickSound);
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }

        public void GuideOpen()
        {
            PlaySound(guideButton.ClickSound);
            GuideSheet.SetActive(!GuideSheet.activeSelf);
            MenuList.SetActive(!MenuList.activeSelf);
        }

        public void GuideClose()
        {
            PlaySound(guideCloseClip, 0.38f);
            GuideSheet.SetActive(false);
            MenuList.SetActive(true);
        }

        public void ExitGame()
        {
            PlaySound(exitButton.ClickSound);
            Application.Quit();
        }

        private void PlaySound(AudioClip clip, float delaySeconds = 0.0f)
        {
            Debug.Log("Clip playing: " + clip);
            src.clip = clip;
            src.time = delaySeconds;
            src.Play();
        }
    }
}