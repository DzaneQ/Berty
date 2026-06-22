using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Utility
{

    public abstract class ServerManagerSingleton<T> : NetworkBehaviour where T : NetworkBehaviour
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (NetworkManager.Singleton == null || !NetworkManager.Singleton.IsServer)
                {
                    return null;
                }
                if (s_instance == null)
                {
                    T[] objects = FindObjectsOfType<T>();
                    switch (objects.Length)
                    {
                        case 0:
                            GameObject managerObject = new();
                            //managerObject.hideFlags = HideFlags.HideInHierarchy;
                            s_instance = managerObject.AddComponent<T>();
                            break;
                        case 1:
                            s_instance = objects[0].GetComponent<T>();
                            break;
                        default:
                            throw new Exception($"There are {objects.Length} singletons of type {typeof(T).Name}");
                    }
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
