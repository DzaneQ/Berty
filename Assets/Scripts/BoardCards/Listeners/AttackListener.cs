using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Managers;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Entities;
using Berty.Grid.Managers;
using Berty.UI.Card.Managers;
using Newtonsoft.Json.Linq;
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Berty.BoardCards.Listeners
{
    public class AttackListener : MonoBehaviour
    {
        private BoardCardCore card;
        private Game game;

        private void Start()
        {
            card = GetComponent<BoardCardCore>();
            game = CoreManager.Instance.Game;
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
            card.StatChange.AdvanceHealth(-modifiedAttackerStrength, attacker, true); // Take damage
            //Debug.Log($"{card.name} has {card.BoardCard.Stats.Health} health");
            if (!card.BoardCard.CharacterConfig.CanRiposte(distanceToAttacker)) return;
            int modifiedRiposteStrength = ModifyStatChangeManager.Instance.GetModifiedStrengthForAttack(attacker, card);
            attacker.StatChange.AdvanceHealth(-modifiedRiposteStrength, card, true); // Do riposte
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
            defender.StatChange.AdvanceHealth(-modifiedStrength, card, true);
            //Debug.Log($"{defender.name} has {defender.BoardCard.Stats.Health} health after being attacked by {card.name}");
        }

        private void HandleDirectAttackWitness(object sender, EventArgs args)
        {
            BoardCardCore attacker = (BoardCardCore)sender;
            BoardCardCore witness = card;

            if (witness == attacker) HandleDirectAttackSelf();

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
            switch (card.BoardCard.GetSkill())
            {
                case SkillEnum.BigMadB:
                    card.StatChange.AdvanceDexterity(-1, null);
                    card.StatChange.AdvanceStrength(1, null);
                    break;
                case SkillEnum.PrezydentBert:
                    card.StatChange.AdvancePower(-1, null);
                    break;
            }
        }

        private void HandleBertaAmazonkaEffect(BoardCardCore target, BoardCardCore bertaAmazonka)
        {
            if (bertaAmazonka.BoardCard.GetSkill() != SkillEnum.BertaAmazonka)
                throw new Exception($"BertaAmazonka effect is casted by {bertaAmazonka.BoardCard.CharacterConfig.Name}");
            Vector2Int distance = target.BoardCard.GetDistanceTo(bertaAmazonka.BoardCard); // According to the target's direction, not BertaAmazonka's
            if (distance.x == 0 && distance.y != 0)
            {
                BoardField neighbor = game.Grid.GetFieldDistancedFromCardOrNull(-1, 0, target.BoardCard);
                if (neighbor != null && neighbor.IsOccupied()) 
                    BoardCardCollectionManager.Instance.GetCoreFromEntityOrThrow(neighbor.OccupantCard).StatChange.AdvanceHealth(-1, bertaAmazonka);
                neighbor = game.Grid.GetFieldDistancedFromCardOrNull(1, 0, target.BoardCard);
                if (neighbor != null && neighbor.IsOccupied())
                    BoardCardCollectionManager.Instance.GetCoreFromEntityOrThrow(neighbor.OccupantCard).StatChange.AdvanceHealth(-1, bertaAmazonka);
            }
            else if (distance.x != 0 && distance.y == 0)
            {
                BoardField neighbor = game.Grid.GetFieldDistancedFromCardOrNull(0, -1, target.BoardCard);
                if (neighbor != null && neighbor.IsOccupied())
                    BoardCardCollectionManager.Instance.GetCoreFromEntityOrThrow(neighbor.OccupantCard).StatChange.AdvanceHealth(-1, bertaAmazonka);
                neighbor = game.Grid.GetFieldDistancedFromCardOrNull(0, 1, target.BoardCard);
                if (neighbor != null && neighbor.IsOccupied())
                    BoardCardCollectionManager.Instance.GetCoreFromEntityOrThrow(neighbor.OccupantCard).StatChange.AdvanceHealth(-1, bertaAmazonka);
            }
            else throw new Exception($"Target shouldn't be distanced from BertaAmazonka by: {distance.x}, {distance.y}");
        }    
    }
}