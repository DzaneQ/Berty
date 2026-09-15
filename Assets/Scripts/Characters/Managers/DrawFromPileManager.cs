using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Entities;
using Berty.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berty.Characters.Managers
{
    public class DrawFromPileManager : ManagerSingleton<DrawFromPileManager>, IDrawFromPileManager
    {
        private CardPile cardPile;

        protected override void Awake()
        {
            base.Awake();
            cardPile = EntityLoadManager.Instance.Game.CardPile;
        }

        public void PutRandomKidOrDeactivate(BoardCardBehaviour card, DirectionEnum direction, AlignmentEnum align)
        {
            CharacterConfig kid = cardPile.GetRandomKidFromPile();
            if (kid == null) card.Activation.DeactivateCard();
            else
            {
                card.BoardCard.DeactivateCard();

                // Activate new kid card
                card.EntityHandler.LoadBoardCardEntity(kid, align);
                card.BoardCard.SetDirection(direction);
                card.Bars.UpdateBars();
                EventManager.Instance.RaiseOnNewCharacter(card);
            }
            ManagerLocator.CheckpointManagerInstance.HandleIfRequested();
        }
    }
}
