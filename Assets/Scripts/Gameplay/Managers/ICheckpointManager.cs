namespace Berty.Gameplay.Managers
{
    public interface ICheckpointManager // TODO: Change to abstract class since methods are similar
    {
        void RequestCheckpoint();

        void HandleIfRequested();
    }
}
