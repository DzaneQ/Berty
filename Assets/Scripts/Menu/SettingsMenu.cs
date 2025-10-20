using Berty.Settings;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Berty.Menu
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Slider volumeSlider;
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        private List<Resolution> resolutionOptions;

        public void Start()
        {
            InitializeResolutionOptions();
        }

        private void InitializeResolutionOptions()
        {
            Resolution[] resolutions = Screen.resolutions;
            List<string> resolutionStrings = new();
            resolutionOptions = new();
            int currentResolutionIndex = 0;
            foreach (Resolution res in resolutions)
            {
                string resolutionString = res.width.ToString() + " x " + res.height.ToString();
                resolutionStrings.Add(resolutionString);
                resolutionOptions.Add(res);
                if (Screen.currentResolution.width == res.width && Screen.currentResolution.height == res.height) currentResolutionIndex = resolutionStrings.Count - 1;

            }
            resolutionDropdown.AddOptions(resolutionStrings);
            resolutionDropdown.value = currentResolutionIndex;
        }

        public void UpdateResolution()
        {
            int index = resolutionDropdown.value;
            Screen.SetResolution(resolutionOptions[index].width, resolutionOptions[index].height, true);
        }

        public void UpdateVolume()
        {
            SettingsManager.Instance.SetVolume(volumeSlider.value);
        }
    }
}
