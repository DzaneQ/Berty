using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Unity.Netcode;

namespace Berty.Utility
{

    public abstract class RpcManagerSingleton<T> : NetworkSingleton<T> where T : NetworkSingleton<T>
    {
        protected Game Game { get; private set; }

        protected override bool IsNetworkConnected()
        {
            return NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsClient;
        }

        protected override bool HasRequiredComponents()
        {
            return GetComponent<NetworkObject>() != null;
        }

        public virtual void OnInitializeScene()
        {
            Game = EntityLoadManager.Instance.Game;
        }
    }
}
