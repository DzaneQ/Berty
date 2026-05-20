using Berty.Enums;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Gameplay.Managers
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
                    _myAlignment = NetworkManager.Singleton.LocalClientId == NetworkManager.Singleton.ConnectedClientsIds.First() 
                        ? AlignmentEnum.Player : AlignmentEnum.Opponent;
                }
                return _myAlignment;
            }
        }
    }
}