using Berty.Gameplay.Init;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Network.Init
{
    public class NetworkSceneInit : MonoBehaviour
    {
        private void Start()
        {
            NetworkManager.Singleton.OnConnectionEvent += HandleClientConnected;
#if UNITY_EDITOR
            NetworkManager.Singleton.StartHost();
#else
            NetworkManager.Singleton.StartClient();
#endif
        }

        private void HandleClientConnected(NetworkManager manager, ConnectionEventData data)
        {
            if (data.EventType != ConnectionEvent.ClientConnected) return;
            int clientCount = manager.ConnectedClientsIds.Count;
            if (clientCount > 2) throw new Exception($"Too many connected clients: {clientCount}");
            if (clientCount < 2) return;
            if (!manager.IsServer) return; // run from server only
            gameObject.AddComponent<SceneInit>();
        }
    }
}
