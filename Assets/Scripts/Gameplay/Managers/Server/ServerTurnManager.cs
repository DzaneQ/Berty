/*using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Network.Managers.Shared;
using Berty.UI.Managers;
using Berty.Utility;
using UnityEngine;

namespace Berty.Gameplay.Managers.Server
{
    public class ServerTurnManager : ServerManagerSingleton<ServerTurnManager>
    {
        private Game game;


        protected override void Awake()
        {
            InitializeSingleton();
            game = EntityLoadManager.Instance.Game;
        }

        public void EndTurn()
        {
            game.SwitchAlignment();
            SyncGameEntityToClients.Instance.Sync();
            SharedTurnManager.Instance.StartNewTurnClientRpc();
        }

        public void EndTheGame()
        {
            AlignmentEnum winner = game.Grid.WinningSide();
            if (winner == AlignmentEnum.None) winner = game.CurrentAlignment;
            OverlayObjectManager.Instance.DisplayGameOverScreen(winner);
        }
    }
}*/
