using Berty.BoardCards.ConfigData;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
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

        void Awake()
        {
            InitializeGameEntity();
            InitializeHandCardObjects();
            //InitializeFieldBehaviours();
        }

        void Start()
        {
            if (StartGameBufferManager.Instance.IsStartingNewGame()) StartTheGame();
            Destroy(gameObject);
        }

        private void InitializeGameEntity()
        {
            initGame = EntityLoadManager.Instance.Game;
        }

        private void InitializeHandCardObjects()
        {
            HandCardInitialization init = gameObject.GetComponent<HandCardInitialization>();
            if (init == null) return;
            GameObject stackForHandCards = ObjectReadManager.Instance.HandCardObjectCollection;
            List<HandCardBehaviour> handCardBehaviourCollection = init.InitializeAllCharacterCards();
            HandCardCollection collectionComponent = stackForHandCards.GetComponent<HandCardCollection>();
            collectionComponent.InitializeCollection(handCardBehaviourCollection);
            Destroy(init);
        }   

        private void StartTheGame()
        {
            EventManager.Instance.RaiseOnNewTurn();
        }
    }
}
