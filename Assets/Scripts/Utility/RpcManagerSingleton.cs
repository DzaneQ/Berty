using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Utility
{

    public abstract class RpcManagerSingleton<T> : NetworkSingleton<T> where T : NetworkSingleton<T>
    {
        protected override bool IsNetworkConnected()
        {
            return NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient;
        }

        protected override bool HasRequiredComponents()
        {
            return GetComponent<NetworkObject>() != null;
        }
    }
}
