using Berty.Enums;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Network.Managers
{
    public class NetworkVariableManager : ClientManagerSingleton<NetworkVariableManager>
    {
        public NetworkVariable<AlignmentEnum> CurrentAlignment = new NetworkVariable<AlignmentEnum>(AlignmentEnum.None, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);
    }
}
