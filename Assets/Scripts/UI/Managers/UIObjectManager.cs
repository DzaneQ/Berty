using Berty.Utility;
using UnityEngine;

namespace Berty.UI.Managers
{
    public abstract class UIObjectManager<T> : ManagerSingleton<T> where T : MonoBehaviour
    {
        protected GameObject canvasObject;

        protected override void Awake()
        {
            base.Awake();
            canvasObject = FindObjectOfType<Canvas>().gameObject;
        }
    }
}
