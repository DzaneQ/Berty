using Berty.Gameplay;
using Berty.UI.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Berty.Inputs
{
    public class CloseGame : MonoBehaviour
    {
        public void CardClick()
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }
}
