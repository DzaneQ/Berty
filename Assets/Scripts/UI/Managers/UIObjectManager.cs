using Berty.Utility;
using UnityEngine;

namespace Berty.UI.Managers
{
    public abstract class UIObjectManager<T> : ManagerSingleton<T> where T : ManagerSingleton<T>
    {
        protected GameObject canvasObject;

        protected override void Awake()
        {
            base.Awake();
            canvasObject = FindAnyObjectByType<Canvas>().gameObject;
        }
    }
}
