using Berty.BoardCards.ConfigData;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.UI.Card.Collection;
using Berty.UI.Card.Entities;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Berty.UI.Managers
{
    public class OverlayObjectManager : UIObjectManager<OverlayObjectManager>
    {
        private CardPile cardPile;
        private HandCardCollection behaviourCollection;

        protected override void Awake()
        {
            base.Awake();
            cardPile = EntityLoadManager.Instance.Game.CardPile;
            behaviourCollection = ObjectReadManager.Instance.HandCardObjectCollection.GetComponent<HandCardCollection>();
        }

        public void DisplayGameOverScreen(bool isTheWinner)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/GameOver");
            TMP_Text endingMessage = prefab.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
            endingMessage.text = isTheWinner
                ? GameLanguageManager.Instance.GetTextFromKey("win")
                : GameLanguageManager.Instance.GetTextFromKey("lose");
            Instantiate(prefab, canvasObject.transform);
        }

        public void DisplayDeadCardsScreen() // BUG: During revival, you can interact with hand cards and put them on field.
        {
            if (!ManagerLocator.TurnManagerInstance.IsItMyTurn()) return;
            GameObject screen = ObjectReadManager.Instance.DeadCardsScreen;
            foreach (CharacterConfig deadCard in cardPile.DeadCards)
            {
                Transform card = behaviourCollection.GetBehaviourFromCharacterConfig(deadCard).transform;
                card.SetParent(screen.transform, false);
            }
            screen.SetActive(true);
        }

        public void HideDeadCardsScreen()
        {
            GameObject screen = ObjectReadManager.Instance.DeadCardsScreen;
            Transform destination = ObjectReadManager.Instance.HandCardObjectCollection.transform;
            screen.SetActive(false);
            for (int i = screen.transform.childCount - 1; 0 <= i; i--)
            {
                screen.transform.GetChild(i).SetParent(destination, false);
            }
        }
    }
}
