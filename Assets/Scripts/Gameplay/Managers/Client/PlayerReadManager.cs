using Berty.Enums;
using Berty.Utility;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

namespace Berty.Gameplay.Managers.Client
{
    public class PlayerReadManager : ClientManagerSingleton<PlayerReadManager>
    {
        private AlignmentEnum _myAlignment = AlignmentEnum.None;

        public AlignmentEnum MyAlignment
        {
            get
            {
                if (_myAlignment == AlignmentEnum.None)
                {
                    IReadOnlyList<ulong> connectedIds = NetworkManager.Singleton.ConnectedClientsIds;
                    if (connectedIds.Count < 2) return AlignmentEnum.None;
                    _myAlignment = NetworkManager.Singleton.LocalClientId == NetworkManager.Singleton.ConnectedClientsIds.First() 
                        ? AlignmentEnum.Player : AlignmentEnum.Opponent;
                }
                return _myAlignment;
            }
        }
    }
}