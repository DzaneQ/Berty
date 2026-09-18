using Berty.BoardCards.Managers;
using Berty.Enums;

namespace Berty.BoardCards.Button
{
    public class RotateRight : CardButton
    {
        private void OnMouseDown()
        {
            BoardCardActionManager.Instance.OrderRotateCard(card, GetName());
        }

        protected override bool CanNavigate() => true;

        public override NavigationEnum GetName() => NavigationEnum.RotateRight;
    }
}