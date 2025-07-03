using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Utility
{

    public abstract class ManagerSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_instance;

        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    T[] objects = FindObjectsOfType<T>();
                    if (objects.Length == 0)
                    {
                        GameObject managerObject = GameObject.Find("ManagerSystem");
                        if (managerObject == null) managerObject = new GameObject("ManagerSystem");
                        if (s_instance == null) s_instance = managerObject.AddComponent<T>();
                        else s_instance = managerObject.GetComponent<T>();
                    }
                    if (objects.Length > 1) throw new Exception($"Too many singletons of type {typeof(T).Name}");
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
