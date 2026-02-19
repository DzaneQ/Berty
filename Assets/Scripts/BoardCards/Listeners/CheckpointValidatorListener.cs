using Berty.BoardCards.Behaviours;
using Berty.Enums;
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
            if (IsEligibleForCheckpoint()) return;
            args.IsRestricted = true;
            Debug.Log(BoardCard.CharacterConfig.Name + " has restricted checkpointing.");

        }

        private bool IsEligibleForCheckpoint()
        {
            if (Navigation.IsCardAnimating()) return false;
            if (BoardCard.Stats.Health <= 0) return false;
            if (BoardCard.Stats.Power <= 0) return false;
            if (BoardCard.GetSkill() == SkillEnum.BertWick && BoardCard.Stats.Dexterity <= 0) return false;
            return true;
        }
    }
}