using Berty.BoardCards.Behaviours;
using Berty.Gameplay.Managers;
using UnityEngine;

namespace Berty.BoardCards.Listeners
{
    public class CheckpointValidatorListener : BoardCardBehaviour
    {
        private void OnEnable()
        {
            EventManager.Instance.OnCheckpointRequest += HandleCheckpointRequest;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnCheckpointRequest -= HandleCheckpointRequest;
        }

        private void HandleCheckpointRequest(object sender, ValidateOutputEventArgs args)
        {
            if (args.IsRestricted) return;
            if (!Core.IsEligibleForCheckpoint()) args.IsRestricted = true;
        }
    }
}