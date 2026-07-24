using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card;
using Berty.Utility;

namespace Berty.Characters.Managers
{
    public class ApplyManualEffectManager : ManagerSingleton<ApplyManualEffectManager>, IApplyManualEffectManager
    {
        private Game Game { get; set; }

        protected override void Awake()
        {
            InitializeSingleton();
            Game = EntityLoadManager.Instance.Game;
        }

        public void ReviveCard(HandCardBehaviour handCardObject)
        {
            Status revival = Game.GetStatusByNameOrThrow(StatusEnum.RevivalSelect);
            Game.CardPile.ReviveCard(handCardObject.Character, revival.GetAlign());
            ManagerLocator.HandCardObjectManagerInstance.AddCardObjects();
            StatusManager.Instance.RemoveStatus(revival);
        }

        public void EnhanceCard(BoardCardBehaviour boardCardObject)
        {
            Status enhancement = Game.GetStatusByNameOrThrow(StatusEnum.ClickToApplyEffect);
            BoardCardBehaviour source = BoardCardCollectionManager.Instance.GetActiveBehaviourFromEntityOrThrow(enhancement.Provider);
            boardCardObject.EntityHandler.AdvanceStrength(2, source);
            boardCardObject.EntityHandler.AdvanceHealth(1, source);
            StatusManager.Instance.RemoveStatus(enhancement);
        }
    }
}