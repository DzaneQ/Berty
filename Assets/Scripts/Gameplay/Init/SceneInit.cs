using Berty.BoardCards.ConfigData;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using Berty.UI.Card;
using Berty.UI.Card.Collection;
using Berty.UI.Card.Init;
using Berty.UI.Card.Systems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Berty.Gameplay.Init
{
    public class SceneInit : MonoBehaviour
    {
        private Game initGame;

        void Awake()
        {
            InitializeGameEntity();
            InitializeHandCardObjectsAndCardPileEntity();
            InitializeFieldCollection();
        }

        void Start()
        {
            StartTheGame();
            Destroy(gameObject);
        }

        private void InitializeGameEntity()
        {
            initGame = CoreManager.Instance.Game;
        }

        private void InitializeHandCardObjectsAndCardPileEntity()
        {
            HandCardInitialization init = gameObject.GetComponent<HandCardInitialization>();
            if (init == null) return;
            GameObject stackForHandCards = ObjectReadManager.Instance.HandCardObjectCollection;
            List<CharacterConfig> initCardPile = init.InitializeAllCharacterCards(stackForHandCards, out List<HandCardBehaviour> handCardBehaviourCollection);
            initGame.CardPile.InitializeCardPile(initCardPile);
            HandCardCollection collectionComponent = stackForHandCards.GetComponent<HandCardCollection>();
            collectionComponent.InitializeCollection(handCardBehaviourCollection);
            Destroy(init);
        }

        private void InitializeFieldCollection()
        {
            GameObject fieldBoard = ObjectReadManager.Instance.FieldBoard;
            List<FieldBehaviour> fieldBehaviourCollection = fieldBoard.GetComponentsInChildren<FieldBehaviour>().ToList();
            FieldCollection collectionComponent = fieldBoard.GetComponent<FieldCollection>();
            collectionComponent.InitializeCollection(fieldBehaviourCollection);
        }

        private void StartTheGame()
        {
            EventManager.Instance.RaiseOnNewTurn();
        }
    }
}
