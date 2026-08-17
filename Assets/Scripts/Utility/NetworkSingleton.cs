using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Utility
{

    public abstract class NetworkSingleton<T> : NetworkBehaviour where T : NetworkSingleton<T>
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
                    T[] objects = FindObjectsByType<T>();
                    switch (objects.Length)
                    {
                        case 0:
                            GameObject managerObject = new();
                            managerObject.hideFlags = HideFlags.HideInHierarchy;
                            s_instance = managerObject.AddComponent<T>();
                            break;
                        case 1:
                            s_instance = objects[0];
                            break;
                        default:
                            Debug.LogWarning($"There are {objects.Length} network singletons of type {typeof(T).Name}. Destroying extras.");

                            foreach (T obj in objects)
                            {
                                if (obj.HasRequiredComponents())
                                {
                                    s_instance = obj;
                                    break;
                                }
                            }

                            if (s_instance == null)
                            {
                                throw new InvalidOperationException("No instance of type " + typeof(T).Name + " has the required components.");
                            }

                            foreach (T obj in objects)
                            {
                                if (obj != s_instance) Destroy(obj);
                            }
                            break;
                    }

                    if (!s_instance.HasRequiredComponents())
                    {
                        s_instance = null;
                        throw new InvalidOperationException("The singleton instance of type " + typeof(T).Name + " does not have the required components.");
                    }
                }

                if (!s_instance.IsNetworkConnected())
                {
                    return null;
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

        protected abstract bool IsNetworkConnected();

        protected virtual bool HasRequiredComponents()
        {
            return true;
        }

        protected void InitializeSingleton()
        {
            Instance = this as T;
        }
    }
}
