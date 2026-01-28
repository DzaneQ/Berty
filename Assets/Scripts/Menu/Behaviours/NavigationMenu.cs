using Berty.Gameplay.Managers;
using Berty.Menu.Listeners;
using Berty.Settings;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Berty.Menu.Behaviours
{
    public class NavigationMenu : MonoBehaviour
    {
        [SerializeField] private Image GuideSheet;
        [SerializeField] private GameObject SettingsPanel;
        [SerializeField] private GameObject MenuList;
        [SerializeField] private GameObject LoadButton;
        [SerializeField] private GameObject ExitButton;
        [SerializeField] private AudioSource guideCloseSource;
        private AudioSource src;
        private ButtonFunction playButton;
        private ButtonFunction guideButton;
        private ButtonFunction exitButton;

        private void Start()
        {
            src = GetComponent<AudioSource>();
            src.volume = SettingsManager.Instance.Volume;
            playButton = MenuList.transform.GetChild(0).GetComponent<ButtonFunction>();
            guideButton = MenuList.transform.GetChild(2).GetComponent<ButtonFunction>();
            exitButton = ExitButton.GetComponent<ButtonFunction>();

            if (SaveLoadManager.Instance.IsSaveFileExisting())
            {
                LoadButton.SetActive(true);
            }

            if (IsOnPlayerPlatform())
            {
                ExitButton.SetActive(true);
            }
        }

        public void Play()
        {
            PlaySound(playButton.ClickSound);
            StartGameBufferManager.Instance.SetLoading(false);
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }

        public void Load()
        {
            PlaySound(playButton.ClickSound);
            StartGameBufferManager.Instance.SetLoading(true);
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }

        public void GuideOpen()
        {
            PlaySound(guideButton.ClickSound);
            GuideSheet.enabled = true;
            MenuList.SetActive(false);
        }

        public void GuideClose()
        {
            if (!GuideSheet.isActiveAndEnabled) return;
            guideCloseSource.Play();
            GuideSheet.enabled = false;
            MenuList.SetActive(true);
        }

        public void SettingsOpen()
        {
            PlaySound(guideButton.ClickSound);
            SettingsPanel.SetActive(true);
            MenuList.SetActive(false);
        }

        public void SettingsClose()
        {
            PlaySound(guideButton.ClickSound);
            SettingsPanel.SetActive(false);
            MenuList.SetActive(true);
        }

        public void ExitGame()
        {
            PlaySound(exitButton.ClickSound);
            Application.Quit();
        }

        private void PlaySound(AudioClip clip, float delaySeconds = 0.0f)
        {
            //Debug.Log("Clip playing: " + clip);
            src.clip = clip;
            src.time = delaySeconds;
            src.Play();
        }

        private bool IsOnPlayerPlatform()
        {
            if (Application.platform == RuntimePlatform.WindowsPlayer) return true;
            if (Application.platform == RuntimePlatform.LinuxPlayer) return true;
            if (Application.platform == RuntimePlatform.OSXPlayer) return true;
            return false;
        }
    }
}