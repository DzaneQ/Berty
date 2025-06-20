using Berty.BoardCards.ConfigData;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card;
using Berty.UI.Card.Collection;
using Berty.UI.Card.Init;
using Berty.UI.Card.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Init
{
    public class SceneInit : MonoBehaviour
    {
        private Game initGame;

        void Awake()
        {
            InitializeGameEntity();
            InitializeHandCardObjectsAndCardPileEntity();
        }

        void Start()
        {
            StartTheGame();
            Destroy(this);
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

        private void StartTheGame()
        {
            EventManager.Instance.RaiseOnNewTurn();
        }
    }
}
