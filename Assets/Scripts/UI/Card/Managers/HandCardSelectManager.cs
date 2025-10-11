using Berty.Gameplay.Managers;
using Berty.UI.Card.Collection;
using Berty.UI.Managers;

namespace Berty.UI.Card.Managers
{
    public class HandCardSelectManager : UIObjectManager<HandCardSelectManager>
    {
        private HandCardCollection behaviourCollection;

        protected override void Awake()
        {
            base.Awake();
            behaviourCollection = ObjectReadManager.Instance.HandCardObjectCollection.GetComponent<HandCardCollection>();
        }

        public void ChangeSelection(HandCardBehaviour card)
        {
            if (SelectionManager.Instance.IsSelected(card.Character))
            {
                UnselectCard(card);
            }
            else if (SelectionManager.Instance.CanSelectCard())
            {
                SelectionManager.Instance.SelectCard(card.Character);
                card.ShowObjectAsSelected();
            }
        }

        public void UnselectCard(HandCardBehaviour card)
        {
            if (!SelectionManager.Instance.IsSelected(card.Character)) return;
            SelectionManager.Instance.UnselectCard(card.Character);
            card.ShowObjectAsUnselected();
        }

        public void ClearSelection()
        {
            foreach (HandCardBehaviour selectedCard in behaviourCollection.GetBehavioursFromCharacterConfigs(SelectionManager.Instance.SelectedCards))
            {
                SelectionManager.Instance.UnselectCard(selectedCard.Character);
                selectedCard.MakeObjectUnselectedWithoutAnimation();
            }
        }
    }
}
