using UnityEngine;

namespace Berty.UI.Listeners
{
    public class DeactivateObjectInput : MonoBehaviour
    {
        [SerializeField] private GameObject targetObject;

        public void OnClick()
        {
            targetObject.SetActive(false);
        }
    }
}
