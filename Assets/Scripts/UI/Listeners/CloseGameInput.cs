using Berty.Gameplay;
using Berty.UI.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Berty.UI.Listeners
{
    public class CloseGameInput : MonoBehaviour
    {
        public void OnClick()
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }
}
