using Berty.BoardCards.Behaviours;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using System;
using UnityEngine;
using Berty.BoardCards.Managers;
using Berty.Characters.Managers;

namespace Berty.BoardCards.Listeners
{
    public class CharacterSkillListener : MonoBehaviour
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
            EventManager.Instance.OnNewCharacter += HandleNewCharacter;
            EventManager.Instance.OnMovedCharacter += HandleMovedCharacter;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnNewCharacter -= HandleNewCharacter;
            EventManager.Instance.OnMovedCharacter -= HandleMovedCharacter;
        }

        private void HandleNewCharacter(object sender, EventArgs args)
        {
            BoardCardCore newCharacter = (BoardCardCore)sender;
            HandleSkillEventManager.Instance.HandleNewCardWitness(core, newCharacter);
        }

        // BUG: This method is run even when cancelling move during NewTransform.
        private void HandleMovedCharacter(object sender, EventArgs args)
        {
            BoardCardCore movedCharacter = (BoardCardCore)sender;
            HandleSkillEventManager.Instance.HandleMovedCardWitness(core, movedCharacter);
        }
    }
}