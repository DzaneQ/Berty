using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Init;
using Berty.Gameplay.Managers;
using Berty.Network.Managers;
using Berty.UI.Card;
using Berty.UI.Card.Collection;
using Berty.UI.Card.Init;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Network.Init
{
    public class NetworkSceneInit : NetworkBehaviour
    {
        private void Start()
        {
            NetworkManager.Singleton.OnConnectionEvent += HandleClientConnected;
#if UNITY_EDITOR
            NetworkManager.Singleton.StartHost();
#else
            NetworkManager.Singleton.StartClient();
#endif
            InitializeLocalScene();
        }

        private void HandleClientConnected(NetworkManager manager, ConnectionEventData data)
        {
            if (!manager.IsServer) return; // run from server only
            if (data.EventType != ConnectionEvent.ClientConnected) return;
            IReadOnlyList<ulong> connectedClients = manager.ConnectedClientsIds;
            int clientCount = connectedClients.Count;
            if (clientCount > 2) throw new Exception($"Too many connected clients: {clientCount}");
            if (clientCount < 2) return;
            PlayerReadManager.Instance.InitializeAlignmentsForClients(connectedClients);
            InitializeGameEntity();
            string dataStr = ProcessGameDataManager.Instance.GetGameEntityAsString();
            InitializeSceneClientRpc(dataStr);
        }

        private void InitializeLocalScene()
        {
            InitializeManagers();
            InitializeLanguage();
        }

        private void InitializeManagers()
        {
            ManagerLocator.InitializeMultiplayer();
        }

        private void InitializeLanguage()
        {
            LanguageInit init = gameObject.GetComponent<LanguageInit>();
            if (init == null) return;
            init.InitializeLanguageDictionary();
            Destroy(init);
        }

        private void InitializeGameEntity()
        {
            Game _ = EntityLoadManager.Instance.Game; // TODO: Stop storing whole entities in client, especially card pile.
        }

        private void InitializeHandCardObjects()
        {
            HandCardInitialization init = gameObject.GetComponent<HandCardInitialization>();
            if (init == null) throw new Exception($"HandCardInitialization component should appear in: {gameObject.name}");
            GameObject stackForHandCards = ObjectReadManager.Instance.HandCardObjectCollection;
            List<HandCardBehaviour> handCardBehaviourCollection = init.InitializeAllCharacterCards();
            HandCardCollection collectionComponent = stackForHandCards.GetComponent<HandCardCollection>();
            collectionComponent.InitializeCollection(handCardBehaviourCollection);
            Destroy(init);
        }

        private void StartTheGame()
        {
            EventManager.Instance.RaiseOnNewTurn();
            Destroy(gameObject);
        }

        [ClientRpc]
        private void InitializeSceneClientRpc(string gameDataStr)
        {
            GameSaveData gameData = ProcessGameDataManager.Instance.GetDataFromString(gameDataStr);
            EntityLoadManager.Instance.LoadGameFromData(gameData);
            InitializeHandCardObjects();
            StartTheGame();
        }
    }
}
