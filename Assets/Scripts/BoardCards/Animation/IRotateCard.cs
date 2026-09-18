namespace Berty.BoardCards.Animation
{
    internal interface IRotateCard
    {
        int CoroutineCount { get; }
        void ByAngleWithoutAnimation(int angle);
        void ByAngle(int angle);
    }
}
