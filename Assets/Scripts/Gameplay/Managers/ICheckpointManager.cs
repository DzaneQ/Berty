using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Utility;
using System;

namespace Berty.Gameplay.Managers
{
    public interface ICheckpointManager
    {
        public void RequestCheckpoint();

        public void HandleIfRequested();
    }
}
