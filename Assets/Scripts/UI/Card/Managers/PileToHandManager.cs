using Berty.UI.Card.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using Berty.Debugging;

namespace Berty.UI.Card.Managers
{
    public class PileToHandManager : ManagerSingleton<PileToHandManager>
    {
        private Game game { get; set; }
        private CardPile cardPile => game.CardPile;

        protected override void Awake()
        {
            InitializeSingleton();
            game = CoreManager.Instance.Game;
        }

        public void PullCardsTo(int capacity)
        {
            AlignmentEnum align = game.CurrentAlignment;
            DebugManager.Instance?.TakeCardIfInPile(align);
            Status extraCardStatus = game.GetStatusByNameAndAlignmentOrNull(StatusEnum.ExtraCardNextTurn, align);
            if (extraCardStatus != null)
            {
                capacity += extraCardStatus.Charges;
                StatusManager.Instance.RemoveStatus(extraCardStatus);
            }
            if (cardPile.PullCardsTo(capacity, align)) HandCardObjectManager.Instance.AddCardObjects();
            else TurnManager.Instance.EndTheGame();
        }
    }
}
