using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Utility
{

    public class ManagerSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T instance { get; private set; }

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    T[] objects = FindObjectsOfType<T>();
                    if (objects.Length == 0)
                    {
                        GameObject managerObject = GameObject.Find("ManagerSystem");
                        if (managerObject == null) instance = new GameObject("ManagerSystem").AddComponent<T>();
                        instance = managerObject.GetComponent<T>();
                        if (instance == null) managerObject.AddComponent<T>();
                    }
                    if (objects.Length > 1) throw new Exception($"Too many singletons of type {typeof(T).Name}");
                }
                return instance;
            }
            private set
            {
                instance = value;
            }
        }

        void Awake()
        {
            InitializeSingleton();
        }

        private void InitializeSingleton()
        {
            Instance = this as T;
        }
    }
}
