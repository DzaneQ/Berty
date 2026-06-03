using Berty.Enums;
using Berty.Network.Managers.Shared;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Gameplay.Managers.Client
{
    public class PlayerReadManager : ClientManagerSingleton<PlayerReadManager>
    {
        private AlignmentEnum _myAlignment = AlignmentEnum.None;

        private AlignmentEnum MyAlignment
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

        public bool IsItMyTurn()
        {
            return IsMyAlignment(SharedTurnManager.Instance.CurrentAlignment);
        }

        public bool IsItNotMyTurn()
        {
            return !IsItMyTurn();
        }

        private bool IsMyAlignment(AlignmentEnum align)
        {
            return MyAlignment == align;
        }
    }
}