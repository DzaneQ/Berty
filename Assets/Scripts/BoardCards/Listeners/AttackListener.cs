using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Managers;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Managers;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Berty.BoardCards.Listeners
{
    public class AttackListener : MonoBehaviour
    {
        private BoardCardCore card;

        private void Start()
        {
            card = GetComponent<BoardCardCore>();
        }

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
            BoardCardCore attacker = (BoardCardCore)sender;
            //Debug.Log($"Handling direct attack by {card.name}");
            if (!args.AttackedFields.Contains(card.BoardCard.OccupiedField)) return;
            //Debug.Log($"{card.name} is attacked");
            Vector2Int distanceToAttacker = card.BoardCard.GetDistanceTo(attacker.BoardCard);
            if (card.BoardCard.CharacterConfig.CanBlock(distanceToAttacker)) return; // Try blocking
            //Debug.Log($"{card.name} with {card.BoardCard.Stats.Health} health takes damage");
            int modifiedAttackerStrength = ModifyStatChangeManager.Instance.GetModifiedStrengthForAttack(card, attacker);
            card.StatChange.AdvanceHealth(-modifiedAttackerStrength, attacker); // Take damage
            //Debug.Log($"{card.name} has {card.BoardCard.Stats.Health} health");
            if (!card.BoardCard.CharacterConfig.CanRiposte(distanceToAttacker)) return;
            int modifiedRiposteStrength = ModifyStatChangeManager.Instance.GetModifiedStrengthForAttack(attacker, card);
            attacker.StatChange.AdvanceHealth(-modifiedRiposteStrength, card); // Do riposte
            //Debug.Log($"{attacker.name} has {card.BoardCard.Stats.Health} health due to riposte");
        }

        private void HandleAttackNewStand(object sender, EventArgs args)
        {
            BoardCardCore defender = (BoardCardCore)sender;
            //Debug.Log($"Attacking {defender.name} by {card.name}");
            if (defender.BoardCard.Align == card.BoardCard.Align) return; // Don't attack allies
            //Debug.Log($"{defender.name} is not {card.name}'s ally");
            if (defender.BoardCard.Stats.Power >= card.BoardCard.Stats.Power) return; // Attack lower power only
            //Debug.Log($"{defender.name} has lower power than {card.name}");
            Vector2Int distanceToDefender = card.BoardCard.GetDistanceTo(defender.BoardCard);
            if (!card.BoardCard.CharacterConfig.CanAttack(distanceToDefender)) return; // Has to be in attack range
            //Debug.Log($"{defender.name} with {defender.BoardCard.Stats.Health} health is attacked by {card.name}");
            int modifiedStrength = ModifyStatChangeManager.Instance.GetModifiedStrengthForAttack(defender, card);
            defender.StatChange.AdvanceHealth(-modifiedStrength, card);
            //Debug.Log($"{defender.name} has {defender.BoardCard.Stats.Health} health after being attacked by {card.name}");
        }

        private void HandleDirectAttackWitness(object sender, EventArgs args)
        {
            BoardCardCore attacker = (BoardCardCore)sender;
            BoardCardCore witness = card;

            if (witness == attacker) HandleDirectAttackSelf(attacker);

            switch (attacker.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.MisiekBert:
                case CharacterEnum.PrezydentBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(witness, attacker);
                    break;
            }
        }

        private void HandleDirectAttackSelf(BoardCardCore skillCard)
        {
            switch (skillCard.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.PrezydentBert:
                    skillCard.StatChange.AdvancePower(-1, null);
                    break;
            }
        }

    }
}