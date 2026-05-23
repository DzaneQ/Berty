using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Utility
{

    public abstract class SharedManagerSingleton<T> : NetworkBehaviour where T : NetworkBehaviour
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (NetworkManager.Singleton == null)
                {
                    return null;
                }
                if (s_instance == null)
                {
                    s_instance = FindObjectOfType<NetworkObject>().gameObject.GetComponent<T>();
                    if (s_instance == null) throw new Exception($"Shared manager {typeof(T).Name} should be pre-exist in SharedManagerSystem on scene.");
                }
                return s_instance;
            }
            private set
            {
                s_instance = value;
            }
        }

        protected virtual void Awake()
        {
            InitializeSingleton();
        }

        protected void InitializeSingleton()
        {
            Instance = this as T;
        }
    }
}
