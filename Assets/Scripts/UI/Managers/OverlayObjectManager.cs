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
            cardPile = CoreManager.Instance.Game.CardPile;
            behaviourCollection = ObjectReadManager.Instance.HandCardObjectCollection.GetComponent<HandCardCollection>();
        }

        public void DisplayGameOverScreen(AlignmentEnum winner)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/GameOver");
            Text endingMessage = prefab.transform.GetChild(0).gameObject.GetComponent<Text>();
            if (winner == AlignmentEnum.Player) endingMessage.text = LanguageManager.Instance.GetTextFromKey("win");
            else if (winner == AlignmentEnum.Opponent) endingMessage.text = LanguageManager.Instance.GetTextFromKey("lose");
            else throw new Exception("Undefined winner.");
            Instantiate(prefab, canvasObject.transform);
        }

        public void DisplayDeadCardsScreen()
        {
            GameObject screen = ObjectReadManager.Instance.DeadCardsScreen;
            foreach (CharacterConfig deadCard in cardPile.DeadCards)
            {
                Transform card = behaviourCollection.GetBehaviourFromCharacterConfig(deadCard).transform;
                card.SetParent(screen.transform);
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
                screen.transform.GetChild(i).SetParent(destination);
            }
        }
    }
}
