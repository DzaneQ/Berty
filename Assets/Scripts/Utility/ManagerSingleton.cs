using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Utility
{

    public abstract class ManagerSingleton<T> : MonoBehaviour where T : ManagerSingleton<T>
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
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
                            s_instance = objects[0].GetComponent<T>();
                            break;
                        default:
                            s_instance = objects[0].GetComponent<T>();

                            Debug.LogWarning($"There are {objects.Length} manager singletons of type {typeof(T).Name}. Destroying extras.");

                            foreach (T obj in objects)
                            {
                                if (obj != s_instance) Destroy(obj);
                            }
                            break;
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
