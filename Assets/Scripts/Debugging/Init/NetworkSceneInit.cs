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
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Debugging.Init
{
    // NOTE: Use this when the launching the game from Multi scene
    public class NetworkSceneInit : NetworkBehaviour
    {
        private const int SCREEN_LABEL_INDEX = 0;

        [SerializeField] private GameObject rpcSystem;
        [SerializeField] private GameObject fieldBoard;

        private List<BoxCollider> _fieldCollidersToUnlock = new();
        private List<MonoBehaviour> _fieldBehavioursToUnlock = new();
        private GameObject _preLabelScreen;

        private void Start()
        {
            NetworkManager.Singleton.OnConnectionEvent += HandleClientConnected;
#if UNITY_EDITOR
            NetworkManager.Singleton.StartHost();
#else
            NetworkManager.Singleton.StartClient();
#endif
            InitializeLocalScene();
            CacheVariables();
        }

        private void HandleClientConnected(NetworkManager manager, ConnectionEventData data)
        {
            if (!manager.IsServer) return; // run from server only
            if (data.EventType != ConnectionEvent.ClientConnected) return;
            IReadOnlyList<ulong> connectedClients = manager.ConnectedClientsIds;
            int clientCount = connectedClients.Count;
            if (clientCount > 2) throw new Exception($"Too many connected clients: {clientCount}");
            if (clientCount < 2) return;
            InitializeGameEntity();
            InitializeManagers();
            PlayerReadManager.Instance.InitializeAlignmentsForClients(connectedClients);
            string dataStr = ProcessGameDataManager.Instance.GetGameEntityAsString();
            InitializeSceneClientRpc(dataStr);
        }

        private void InitializeLocalScene()
        {
            InitializeLanguage();
            InitializeLabelScreen();
        }

        private void CacheVariables()
        {
            foreach (Transform field in fieldBoard.transform)
            {
                _fieldCollidersToUnlock.Add(field.gameObject.GetComponent<BoxCollider>());
                _fieldBehavioursToUnlock.Add(field.gameObject.GetComponent<FieldBehaviour>());
            }
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

        private void InitializeLabelScreen()
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/LabelScreen");
            TMP_Text endingMessage = prefab.transform.GetChild(SCREEN_LABEL_INDEX).GetComponent<TMP_Text>();
            endingMessage.text = GameLanguageManager.Instance.GetTextFromKey("pre_multi");
            _preLabelScreen = Instantiate(prefab, GameObject.FindGameObjectWithTag("MainScreen").transform);
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
            Destroy(_preLabelScreen.gameObject);
            EventManager.Instance.RaiseOnNewTurn();
            Destroy(gameObject);
        }

        [ClientRpc]
        private void InitializeSceneClientRpc(string gameDataStr)
        {
            if (!IsServer)
            {
                GameSaveData gameData = ProcessGameDataManager.Instance.GetDataFromString(gameDataStr);
                EntityLoadManager.Instance.LoadData(gameData);
                InitializeManagers();
            }
            UnlockFields();
            rpcSystem.SendMessage("OnInitializeScene");
            InitializeHandCardObjects();
            StartTheGame();
        }
    }
}
