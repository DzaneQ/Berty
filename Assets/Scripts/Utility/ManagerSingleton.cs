using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Utility
{

    public class ManagerSingleton<T> : MonoBehaviour where T : MonoBehaviour
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
                        if (managerObject == null) _instance = new GameObject("ManagerSystem").AddComponent<T>();
                        _instance = managerObject.GetComponent<T>();
                        if (_instance == null) managerObject.AddComponent<T>();
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
            throw new InvalidOperationException("Single shouldn't be called from its base.");
        }

        protected void InitializeSingleton()
        {
            Instance = this as T;
        }
    }
}
