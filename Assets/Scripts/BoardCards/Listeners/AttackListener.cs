using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Managers;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Entities;
using System;
using UnityEngine;

namespace Berty.BoardCards.Listeners
{
    public class AttackListener : BoardCardBehaviour
    {
        private void OnEnable()
        {
            EventManager.Instance.OnDirectlyAttacked += HandleDirectlyAttacked;
            EventManager.Instance.OnDirectlyAttacked += HandleDirectAttackWitness;
            EventManager.Instance.OnAttackNewStand += HandleAttackNewStand;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnDirectlyAttacked -= HandleDirectlyAttacked;
            EventManager.Instance.OnDirectlyAttacked -= HandleDirectAttackWitness;
            EventManager.Instance.OnAttackNewStand -= HandleAttackNewStand;
        }

        private void HandleDirectlyAttacked(object sender, DirectAttackEventArgs args)
        {
            BoardCardBehaviour attacker = (BoardCardBehaviour)sender;
            if (!args.AttackedFields.Contains(BoardCard.OccupiedField)) return;
            Vector2Int distanceToAttacker = BoardCard.GetDistanceTo(attacker.BoardCard);
            
            // Try blocking
            if (BoardCard.CharacterConfig.CanBlock(distanceToAttacker)) return;

            // Take damage
            int modifiedAttackerStrength = ModifyStatChangeManager.Instance.GetModifiedStrengthForAttack(this, attacker);
            EntityHandler.AdvanceHealth(-modifiedAttackerStrength, attacker, true);
            args.SuccessfullyAttackedCards.Add(this);

            // Try riposte
            if (!BoardCard.CharacterConfig.CanRiposte(distanceToAttacker)) return;
            int modifiedRiposteStrength = ModifyStatChangeManager.Instance.GetModifiedStrengthForAttack(attacker, this);
            attacker.EntityHandler.AdvanceHealth(-modifiedRiposteStrength, this, true);
        }

        private void HandleAttackNewStand(object sender, EventArgs args)
        {
            BoardCardBehaviour defender = (BoardCardBehaviour)sender;
            if (defender.BoardCard.Align == BoardCard.Align) return; // Don't attack allies
            if (defender.BoardCard.Stats.Power >= BoardCard.Stats.Power) return; // Attack lower power only
            Vector2Int distanceToDefender = BoardCard.GetDistanceTo(defender.BoardCard);
            if (!BoardCard.CharacterConfig.CanAttack(distanceToDefender)) return; // Has to be in attack range
            int modifiedStrength = ModifyStatChangeManager.Instance.GetModifiedStrengthForAttack(defender, this);
            defender.EntityHandler.AdvanceHealth(-modifiedStrength, this, true);
        }

        private void HandleDirectAttackWitness(object sender, EventArgs args)
        {
            BoardCardBehaviour attacker = (BoardCardBehaviour)sender;
            BoardCardBehaviour witness = this;

            if (witness.IsEqualTo(attacker)) HandleDirectAttackSelf();

            switch (attacker.BoardCard.GetSkill())
            {
                case SkillEnum.BertaAmazonka:
                    if (attacker.BoardCard.CanAttackCard(witness.BoardCard))
                        HandleBertaAmazonkaEffect(witness, attacker);
                    break;
                case SkillEnum.MisiekBert:
                case SkillEnum.PrezydentBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(witness, attacker);
                    break;
            }
        }

        private void HandleDirectAttackSelf()
        {
            switch (BoardCard.GetSkill())
            {
                case SkillEnum.BigMadB:
                    EntityHandler.AdvanceDexterity(-1, null);
                    EntityHandler.AdvanceStrength(1, null);
                    break;
                case SkillEnum.PrezydentBert:
                    EntityHandler.AdvancePower(-1, null);
                    break;
            }
        }

        private void HandleBertaAmazonkaEffect(BoardCardBehaviour target, BoardCardBehaviour bertaAmazonka)
        {
            if (bertaAmazonka.BoardCard.GetSkill() != SkillEnum.BertaAmazonka)
                throw new Exception($"BertaAmazonka effect is casted by {bertaAmazonka.BoardCard.CharacterConfig.Name}");
            Vector2Int distance = target.BoardCard.GetDistanceTo(bertaAmazonka.BoardCard); // According to the target's direction, not BertaAmazonka's
            if (distance.x == 0 && distance.y != 0)
            {
                BoardField neighbor = game.Grid.GetFieldDistancedFromCardOrNull(-1, 0, target.BoardCard);
                if (neighbor != null && neighbor.IsOccupied()) 
                    BoardCardCollectionManager.Instance.GetActiveBehaviourFromEntityOrThrow(neighbor.OccupantCard).EntityHandler.AdvanceHealth(-1, bertaAmazonka);
                neighbor = game.Grid.GetFieldDistancedFromCardOrNull(1, 0, target.BoardCard);
                if (neighbor != null && neighbor.IsOccupied())
                    BoardCardCollectionManager.Instance.GetActiveBehaviourFromEntityOrThrow(neighbor.OccupantCard).EntityHandler.AdvanceHealth(-1, bertaAmazonka);
            }
            else if (distance.x != 0 && distance.y == 0)
            {
                BoardField neighbor = game.Grid.GetFieldDistancedFromCardOrNull(0, -1, target.BoardCard);
                if (neighbor != null && neighbor.IsOccupied())
                    BoardCardCollectionManager.Instance.GetActiveBehaviourFromEntityOrThrow(neighbor.OccupantCard).EntityHandler.AdvanceHealth(-1, bertaAmazonka);
                neighbor = game.Grid.GetFieldDistancedFromCardOrNull(0, 1, target.BoardCard);
                if (neighbor != null && neighbor.IsOccupied())
                    BoardCardCollectionManager.Instance.GetActiveBehaviourFromEntityOrThrow(neighbor.OccupantCard).EntityHandler.AdvanceHealth(-1, bertaAmazonka);
            }
            else throw new Exception($"Target shouldn't be distanced from BertaAmazonka by: {distance.x}, {distance.y}");
        }    
    }
}