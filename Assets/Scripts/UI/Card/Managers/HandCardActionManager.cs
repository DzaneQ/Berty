using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Entities;
using Berty.Utility;

namespace Berty.UI.Card.Managers
{
    public class HandCardActionManager : ManagerSingleton<HandCardActionManager>
    {
        private Game Game { get; set; }
        private CardPile CardPile => Game.CardPile;

        protected override void Awake()
        {
            InitializeSingleton();
            Game = EntityLoadManager.Instance.Game;
        }

        public void ReviveCard(HandCardBehaviour cardObject)
        {
            Status revival = Game.GetStatusByNameOrThrow(StatusEnum.RevivalSelect);
            CardPile.ReviveCard(cardObject.Character, revival.GetAlign());
            HandCardObjectManager.Instance.AddCardObjects();
            StatusManager.Instance.RemoveStatus(revival);
        }
    }
}
