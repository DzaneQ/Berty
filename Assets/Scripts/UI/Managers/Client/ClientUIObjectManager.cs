using Berty.Utility;
using Unity.Netcode;
using UnityEngine;

namespace Berty.UI.Managers
{
    public abstract class ClientUIObjectManager<T> : ClientManagerSingleton<T> where T : NetworkBehaviour
    {
        protected GameObject canvasObject;

        protected override void Awake()
        {
            base.Awake();
            canvasObject = FindObjectOfType<Canvas>().gameObject;
        }
    }
}
