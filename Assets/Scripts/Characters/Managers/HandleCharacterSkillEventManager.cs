using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Grid.Field.Entities;
using Berty.UI.Card.Collection;
using Berty.Utility;
using System;
using UnityEngine;

namespace Berty.Characters.Managers
{
    public class HandleCharacterSkillEventManager : ManagerSingleton<HandleCharacterSkillEventManager>
    {
        private Game game;

        protected override void Awake()
        {
            base.Awake();
            game = CoreManager.Instance.Game;
        }

        public void HandleDirectAttackWitness(BoardCardCore witness, BoardCardCore attacker)
        {
            switch (attacker.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.MisiekBert:
                    if (game.Grid.AreNeighboring(witness.ParentField.BoardField, attacker.ParentField.BoardField))
                        ApplyMisiekBertEffect(witness);
                    break;
            }
        }

        public void HandleNewCardWitness(BoardCardCore witness, BoardCardCore newCard)
        {
            switch (newCard.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.MisiekBert:
                    if (game.Grid.AreNeighboring(witness.ParentField.BoardField, newCard.ParentField.BoardField))
                        ApplyMisiekBertEffect(witness);
                    break;
            }
        }

        public void HandleMovedCardWitness(BoardCardCore witness, BoardCardCore movedCard)
        {
            switch (movedCard.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.MisiekBert:
                    if (game.Grid.AreNeighboring(witness.ParentField.BoardField, movedCard.ParentField.BoardField))
                        ApplyMisiekBertEffect(witness);
                    break;
            }
        }

        private void ApplyMisiekBertEffect(BoardCardCore card)
        {
            CardNavigationManager.Instance.RotateCard(card, 270);
        }


    }
}