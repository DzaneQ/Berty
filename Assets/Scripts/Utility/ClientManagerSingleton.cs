using Unity.Netcode;

namespace Berty.Utility
{

    public abstract class ClientManagerSingleton<T> : NetworkSingleton<T> where T : NetworkSingleton<T>
    {
        protected override bool IsNetworkConnected()
        {
            return NetworkManager.Singleton.IsClient;
        }
    }
}
