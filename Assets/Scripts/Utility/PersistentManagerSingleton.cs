using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Utility
{

    public abstract class PersistentManagerSingleton<T> : ManagerSingleton<T> where T : ManagerSingleton<T>
    {
        protected override void Awake()
        {
            InitializeSingleton();
            DontDestroyOnLoad(gameObject);
        }
    }
}
