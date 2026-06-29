using Berty.UI.Card.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using Berty.Debugging.Managers;
using UnityEngine;

namespace Berty.UI.Card.Managers
{
    public class PileToHandManager : ManagerSingleton<PileToHandManager>, IPileToHandManager
    {
        private Game game { get; set; }
        private CardPile cardPile => game.CardPile;

        protected override void Awake()
        {
            InitializeSingleton();
            game = EntityLoadManager.Instance.Game;
        }

        public void PullCards()
        {
            AlignmentEnum align = ManagerLocator.TurnManagerInstance.CurrentAlignment;
            int capacity = game.GameConfig.TableCapacity;
            DebugManager instance = DebugManager.Instance;
            if (instance != null) instance.TakeCardIfInPile(align);
            Status extraCardStatus = game.GetStatusByNameAndAlignmentOrNull(StatusEnum.ExtraCardNextTurn, align);
            if (extraCardStatus != null)
            {
                capacity += extraCardStatus.Charges;
                StatusManager.Instance.RemoveStatus(extraCardStatus);
            }
            if (cardPile.PullCardsTo(capacity, align)) ManagerLocator.HandCardObjectManagerInstance.AddCardObjects();
            else ManagerLocator.TurnManagerInstance.EndTheGame();
        }
    }
}
