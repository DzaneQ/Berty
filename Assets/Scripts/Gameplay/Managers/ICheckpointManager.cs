namespace Berty.Gameplay.Managers
{
    public interface ICheckpointManager // TODO: Change to abstract class since methods are similar
    {
        // BUG: KrolPopuBert after dying (when on the opposing side) will leave an exception on turn end
        void RequestCheckpoint();

        void HandleIfRequested();
    }
}
