using Berty.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace Berty.Menu
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField] private Slider volumeSlider;

        public void UpdateVolume()
        {
            SettingsManager.Instance.SetVolume(volumeSlider.value);
        }
    }
}
