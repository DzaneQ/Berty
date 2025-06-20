using Berty.Entities;
using Berty.Enums;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Berty.UI.Managers
{
    public abstract class UIObjectManager<T> : ManagerSingleton<T> where T : MonoBehaviour
    {
        protected GameObject canvasObject;

        protected virtual void Awake()
        {
            InitializeSingleton();
            canvasObject = FindObjectOfType<Canvas>().gameObject;
        }
    }
}
