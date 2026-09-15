using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Berty.Characters.Managers
{
    public interface IDrawFromPileManager
    {
        void PutRandomKidOrDeactivate(BoardCardBehaviour card, DirectionEnum direction, AlignmentEnum align);
    }
}
