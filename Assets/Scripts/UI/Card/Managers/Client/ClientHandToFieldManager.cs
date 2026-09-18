using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using Berty.Network.Managers;
using Berty.Utility;
using System;
using UnityEngine;

namespace Berty.UI.Card.Managers
{
    public class ClientHandToFieldManager : ClientManagerSingleton<ClientHandToFieldManager>, IHandToFieldManager
    {
        protected override void Awake()
        {
            InitializeSingleton();
        }

        public CharacterConfig RemoveSelectedCardFromHand()
        {
            CharacterConfig selectedCard = SelectionManager.Instance.GetSelectedCardOrThrow();
            SelectionManager.Instance.PutSelectedCardAsPending();
            NetworkCardManager.Instance.RemoveCardFromMyHandOrThrow(selectedCard);
            ManagerLocator.HandCardObjectManagerInstance.RemoveCardObjects();
            HandCardSelectManager.Instance.ClearSelection();
            return selectedCard;
        }

        public GameObject ActivateCardOnField(FieldBehaviour field, CharacterConfig cardConfig)
        {
            GameObject newCardObject = GetNewCardObjectOnField(field);
            newCardObject.SetActive(true);
            newCardObject.GetComponent<BoardCardBehaviour>().Activation.HandleNewCardActivated(cardConfig);
            return newCardObject;
        }

        private GameObject GetNewCardObjectOnField(FieldBehaviour field)
        {
            GameObject cardToActivate = field.transform.GetChild(0).GetChild(0).gameObject;
            if (cardToActivate.activeSelf)  // If there's an active card, get a second card
            {
                cardToActivate = ObjectReadManager.Instance.BackupCard;
                if (cardToActivate.activeSelf) throw new Exception($"Backup card named {cardToActivate.name} is already active.");
                cardToActivate.transform.SetParent(field.transform.GetChild(0), false);
            }
            return cardToActivate;
        }
    }
}
