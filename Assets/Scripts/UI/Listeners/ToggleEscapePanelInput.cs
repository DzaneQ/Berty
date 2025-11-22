using Berty.Gameplay.Listeners;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Berty.UI.Listeners
{
    public class ToggleEscapePanelInput : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EscapePanelManager.Instance.ToggleEscapePanel();
            }
        }
    }
}
