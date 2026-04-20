using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Debugging.Listeners
{
#if DEBUG
    public class NetworkButton : MonoBehaviour
    {

        public void StartClient()
        {
            NetworkManager.Singleton.StartClient();
        }
    }
#else
    public class NetworkButton : MonoBehaviour
    {
        public void StartClient() {}
    }
#endif
}
