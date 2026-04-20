using Berty.BoardCards.ConfigData;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using Berty.Network.Init;
using Berty.Settings;
using Berty.UI.Card;
using Berty.UI.Card.Collection;
using Berty.UI.Card.Init;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Gameplay.Init
{
    public class SceneInit : MonoBehaviour
    {
        private Game initGame;
        private bool areCardsInitialized;

        void Awake()
        {
            InitializeNetwork();
            InitializeGameEntity();
            areCardsInitialized = InitializeHandCardObjects();
            InitializeLanguage();
        }

        void Start()
        {
            TryStartingTheGame();
            Destroy(gameObject); // TODO: Adjust to multiplayer that it's on hold to initialize when there are not enough players
        }

        private void InitializeGameEntity()
        {
            initGame = EntityLoadManager.Instance.Game;
        }

        private bool InitializeHandCardObjects()
        {
            HandCardInitialization init = gameObject.GetComponent<HandCardInitialization>();
            if (init == null) return false;
            GameObject stackForHandCards = ObjectReadManager.Instance.HandCardObjectCollection;
            List<HandCardBehaviour> handCardBehaviourCollection = init.InitializeAllCharacterCards();
            HandCardCollection collectionComponent = stackForHandCards.GetComponent<HandCardCollection>();
            collectionComponent.InitializeCollection(handCardBehaviourCollection);
            Destroy(init);
            return true;
        }

        private void InitializeLanguage()
        {
            LanguageInit init = gameObject.GetComponent<LanguageInit>();
            if (init == null) return;
            init.InitializeLanguageDictionary();
            Destroy(init);
        }

        private void InitializeNetwork()
        {
            NetworkInit init = gameObject.GetComponent<NetworkInit>();
            if (init == null) return; // should return on singleplayer mode
            init.InitializeNetwork();
            Destroy(init);
        }

        private bool CanStartTheGame()
        {
            if (!areCardsInitialized) return false;
            return StartGameBufferManager.Instance.IsStartingNewGame();
        }

        private void TryStartingTheGame()
        {
            if (!CanStartTheGame()) return;
            EventManager.Instance.RaiseOnNewTurn();
        }
    }
}
