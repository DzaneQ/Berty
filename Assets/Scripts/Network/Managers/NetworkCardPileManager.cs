using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Gameplay.Managers.Client;
using Berty.Utility;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Network.Managers
{
    public class NetworkCardPileManager : SharedManagerSingleton<NetworkCardPileManager>
    {
        //private readonly NetworkList<> playerTableCharacterNames = new(); // TODO: Add IEquitable representing cards

        [ClientRpc]
        private void AddCardObjectsClientRpc()
        {
            if (ManagerLocator.TurnManagerInstance.IsItNotMyTurn()) return;
            ManagerLocator.HandCardObjectManagerInstance.AddCardObjects();
        }
    }
}
