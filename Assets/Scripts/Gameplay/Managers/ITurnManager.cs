using Berty.Enums;

namespace Berty.Gameplay.Managers
{
    public interface ITurnManager
    {
        AlignmentEnum CurrentAlignment { get; }
        void EndTurn();
        void EndTheGame();
        bool IsItMyTurn();
        bool IsItNotMyTurn() => !IsItMyTurn();
    }
}
