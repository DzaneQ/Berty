using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Init;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
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
        [SerializeField] private GameObject rpcSystem;
        [SerializeField] private GameObject fieldBoard;

        private List<BoxCollider> _fieldCollidersToUnlock = new();
        private List<MonoBehaviour> _fieldBehavioursToUnlock = new();

        private void Start()
        {
            NetworkManager.Singleton.OnConnectionEvent += HandleClientConnected;
#if UNITY_EDITOR
            NetworkManager.Singleton.StartHost();
#else
            NetworkManager.Singleton.StartClient();
#endif
            InitializeLocalScene();
            CacheComponentsToUnlock();
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
            InitializeLanguage();
        }

        private void CacheComponentsToUnlock()
        {
            foreach (Transform field in fieldBoard.transform)
            {
                _fieldCollidersToUnlock.Add(field.gameObject.GetComponent<BoxCollider>());
                _fieldBehavioursToUnlock.Add(field.gameObject.GetComponent<FieldBehaviour>());
            }
        }

        private void InitializeManagers()
        {
            rpcSystem.SetActive(true);
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
            EntityLoadManager.Instance.InitializeGame(); // TODO: Stop storing whole entities in client, especially card pile.
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

        private void UnlockFields()
        {
            foreach (Collider coll in _fieldCollidersToUnlock) coll.enabled = true;
            foreach (MonoBehaviour bhvr in _fieldBehavioursToUnlock) bhvr.enabled = true;
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
            EntityLoadManager.Instance.OverwriteGameFromData(gameData);
            UnlockFields();
            InitializeManagers();
            InitializeHandCardObjects();
            StartTheGame();
        }
    }
}
