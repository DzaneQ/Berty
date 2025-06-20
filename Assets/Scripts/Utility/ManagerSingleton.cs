using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Utility
{

    public abstract class ManagerSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    T[] objects = FindObjectsOfType<T>();
                    if (objects.Length == 0)
                    {
                        GameObject managerObject = GameObject.Find("ManagerSystem");
                        if (managerObject == null) managerObject = new GameObject("ManagerSystem");
                        if (_instance == null) _instance = managerObject.AddComponent<T>();
                        else _instance = managerObject.GetComponent<T>();
                    }
                    if (objects.Length > 1) throw new Exception($"Too many singletons of type {typeof(T).Name}");
                }
                return _instance;
            }
            private set
            {
                _instance = value;
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
