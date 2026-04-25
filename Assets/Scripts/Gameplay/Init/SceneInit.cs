using Berty.BoardCards.ConfigData;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using Berty.Settings;
using Berty.UI.Card;
using Berty.UI.Card.Collection;
using Berty.UI.Card.Init;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Gameplay.Init
{
    public class SceneInit : MonoBehaviour
    {
        void Awake()
        {
            InitializeGameEntity();
            InitializeHandCardObjects();
            InitializeLanguage();
        }

        void Start()
        {
            if (StartGameBufferManager.Instance.IsStartingNewGame()) StartTheGame();
            Destroy(gameObject);
        }

        private void InitializeGameEntity()
        {
            Game _ = EntityLoadManager.Instance.Game;
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

        private void InitializeLanguage()
        {
            LanguageInit init = gameObject.GetComponent<LanguageInit>();
            if (init == null) return;
            init.InitializeLanguageDictionary();
            Destroy(init);
        }

        private void StartTheGame()
        {
            EventManager.Instance.RaiseOnNewTurn();
        }
    }
}
