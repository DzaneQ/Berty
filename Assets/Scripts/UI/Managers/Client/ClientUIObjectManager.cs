using Berty.Utility;
using Unity.Netcode;
using UnityEngine;

namespace Berty.UI.Managers
{
    public abstract class ClientUIObjectManager<T> : ClientManagerSingleton<T> where T : ClientManagerSingleton<T>
    {
        protected GameObject canvasObject;

        protected override void Awake()
        {
            base.Awake();
            canvasObject = FindAnyObjectByType<Canvas>().gameObject;
        }
    }
}
