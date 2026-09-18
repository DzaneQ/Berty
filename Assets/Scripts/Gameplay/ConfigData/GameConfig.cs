using Berty.Enums;

namespace Berty.Gameplay.ConfigData
{
    public class GameConfig
    {
        public int TableCapacity { get; }
        public LanguageEnum Language { get; }
        public float AnimationSeconds { get; }
        public int AlignedCardsToWin { get; }
        public AlignmentEnum StartingAlignment { get; }

        public GameConfig()
        {
            TableCapacity = 6;
            Language = LanguageEnum.Polish;
            AnimationSeconds = 0.15f;
            AlignedCardsToWin = 6;
            StartingAlignment = AlignmentEnum.Opponent;
        }
    }
}
