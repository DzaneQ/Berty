using Berty.BoardCards.Behaviours;
using Berty.UI.Card;

namespace Berty.Characters.Managers
{
    public interface IApplyManualEffectManager
    {
        public void ReviveCard(HandCardBehaviour handCardObject);

        public void EnhanceCard(BoardCardBehaviour boardCardObject);
    }
}
