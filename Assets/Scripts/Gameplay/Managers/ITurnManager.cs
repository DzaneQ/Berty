namespace Berty.Gameplay.Managers
{
    public interface ITurnManager
    {
        void EndTurn();
        void EndTheGame();
        bool IsItMyTurn();
        bool IsItNotMyTurn() => !IsItMyTurn();
    }
}
