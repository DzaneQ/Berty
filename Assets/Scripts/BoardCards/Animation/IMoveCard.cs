using Berty.Grid.Field.Behaviour;

namespace Berty.BoardCards.Animation
{
    internal interface IMoveCard
    {
        int CoroutineCount { get; }
        void ToField(FieldBehaviour field);
    }
}
