using Berty.BoardCards.Behaviours;
using Berty.Enums;

namespace Berty.Characters.Managers
{
    public interface IDrawFromPileManager
    {
        void PutRandomKidOrDeactivate(BoardCardBehaviour card, DirectionEnum direction, AlignmentEnum align);
    }
}
