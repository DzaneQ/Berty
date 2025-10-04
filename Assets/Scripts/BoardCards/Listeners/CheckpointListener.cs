using Berty.BoardCards.Behaviours;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using System;
using UnityEngine;

namespace Berty.BoardCards.Listeners
{
    public class CheckpointListener : MonoBehaviour
    {
        private BoardCardCore core;
        private Game game;

        private void Awake()
        {
            core = GetComponent<BoardCardCore>();
            game = CoreManager.Instance.Game;
        }

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
            if (!core.IsEligibleForCheckpoint()) args.IsRestricted = true;
        }
    }
}