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
            EventManager.Instance.OnAttackNewStand += HandleAttackNewStand;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnDirectlyAttacked -= HandleDirectlyAttacked;
            EventManager.Instance.OnAttackNewStand -= HandleAttackNewStand;
        }

        private void HandleDirectlyAttacked(object sender, DirectAttackEventArgs args)
        {
            BoardCardCore attacker = (BoardCardCore)sender;
            //Debug.Log($"Handling direct attack by {card.name}");
            if (!args.AttackedFields.Contains(card.BoardCard.OccupiedField))
            {
                HandleCharacterSkillEventManager.Instance.HandleDirectAttackWitness(card, attacker);
                return;
            }
            //Debug.Log($"{card.name} is attacked");
            Vector2Int distanceToAttacker = card.BoardCard.GetDistanceTo(attacker.BoardCard);
            if (card.BoardCard.CharacterConfig.CanBlock(distanceToAttacker)) // Try blocking
            {
                HandleCharacterSkillEventManager.Instance.HandleDirectAttackWitness(card, attacker);
                return;
            }
            //Debug.Log($"{card.name} with {card.BoardCard.Stats.Health} health takes damage");
            StatChangeManager.Instance.AdvanceHealth(card, -attacker.BoardCard.Stats.Strength); // Take damage
            //Debug.Log($"{card.name} has {card.BoardCard.Stats.Health} health");
            if (!card.BoardCard.CharacterConfig.CanRiposte(distanceToAttacker))
            {
                HandleCharacterSkillEventManager.Instance.HandleDirectAttackWitness(card, attacker);
                return;
            }
            StatChangeManager.Instance.AdvanceHealth(attacker, -card.BoardCard.Stats.Strength); // Do riposte
            //Debug.Log($"{attacker.name} has {card.BoardCard.Stats.Health} health due to riposte");
            HandleCharacterSkillEventManager.Instance.HandleDirectAttackWitness(card, attacker);
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
            StatChangeManager.Instance.AdvanceHealth(defender, -card.BoardCard.Stats.Strength);
            //Debug.Log($"{defender.name} has {defender.BoardCard.Stats.Health} health after being attacked by {card.name}");
        }
    }
}