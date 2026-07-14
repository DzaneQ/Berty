namespace Berty.Gameplay.Managers
{
    public interface ICheckpointManager
    {
        void RequestCheckpoint();

        void HandleIfRequested();
    }
}
