using Berty.BoardCards.ConfigData;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Collection;
using Berty.UI.Card.Entities;
using Berty.Utility;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UIElements;

namespace Berty.UI.Managers
{
    public class OverlayObjectManager : UIObjectManager<OverlayObjectManager>
    {
        private CardPile cardPile;
        private HandCardCollection behaviourCollection;
        private Collider[] disabledColliders;
        private int[] interactableLayers;

        protected override void Awake()
        {
            base.Awake();
            cardPile = EntityLoadManager.Instance.Game.CardPile;
            behaviourCollection = ObjectReadManager.Instance.HandCardObjectCollection.GetComponent<HandCardCollection>();
            interactableLayers = GetAllInteractableLayers();
        }

        public void DisplayGameOverScreen(bool isTheWinner)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/GameOver");
            TMP_Text endingMessage = prefab.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();
            endingMessage.text = isTheWinner
                ? GameLanguageManager.Instance.GetTextFromKey("win")
                : GameLanguageManager.Instance.GetTextFromKey("lose");
            Instantiate(prefab, canvasObject.transform);
            ToggleAllColliderInputsForPanel(prefab);
        }

        public void DisplayDeadCardsScreen()
        {
            if (!ManagerLocator.TurnManagerInstance.IsItMyTurn()) return;
            GameObject screen = ObjectReadManager.Instance.DeadCardsScreen;
            foreach (CharacterConfig deadCard in cardPile.DeadCards)
            {
                Transform card = behaviourCollection.GetBehaviourFromCharacterConfig(deadCard).transform;
                card.SetParent(screen.transform, false);
            }
            screen.SetActive(true);
            ToggleAllColliderInputsForPanel(screen);
        }

        public void HideDeadCardsScreen()
        {
            GameObject screen = ObjectReadManager.Instance.DeadCardsScreen;
            Transform destination = ObjectReadManager.Instance.HandCardObjectCollection.transform;
            screen.SetActive(false);
            ToggleAllColliderInputsForPanel(screen);
            for (int i = screen.transform.childCount - 1; 0 <= i; i--)
            {
                screen.transform.GetChild(i).SetParent(destination, false);
            }
        }

        // TODO: Refactor to group escape panel, dead panel and game over panel
        protected void ToggleAllColliderInputsForPanel(GameObject panel)
        {
            Collider[] colliders = panel.activeSelf ? FindObjectsByType<Collider>(FindObjectsInactive.Exclude).ToArray() : disabledColliders;
            if (colliders == null)
            {
                if (panel.activeSelf) throw new Exception("Found no colliders for an active panel: " + panel.name);
                return;
            }
            foreach (Collider coll in colliders)
            {
                if (!interactableLayers.Contains(coll.gameObject.layer)) continue;
                coll.enabled = !panel.activeSelf;
            }
            disabledColliders = panel.activeSelf ? colliders : null;
        }

        private int[] GetAllInteractableLayers()
        {
            return new int[] {
                LayerMask.NameToLayer("Card"),
                LayerMask.NameToLayer("Field"),
                LayerMask.NameToLayer("Button")
            };
        }
    }
}
