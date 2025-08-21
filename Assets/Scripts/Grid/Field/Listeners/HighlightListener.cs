using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using Berty.UI.Card.Managers;
using System;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Berty.Grid.Field.Listeners
{
    public class HighlightListener : MonoBehaviour
    {
        private FieldBehaviour field;

        private void Start()
        {
            field = GetComponent<FieldBehaviour>();
        }

        private void OnEnable()
        {
            EventManager.Instance.OnHighlightStart += HandleHighlightStart;
            EventManager.Instance.OnHighlightEnd += HandleHighlightEnd;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnHighlightStart -= HandleHighlightStart;
            EventManager.Instance.OnHighlightEnd -= HandleHighlightEnd;
        }

        private void HandleHighlightStart(object sender, DirectAttackEventArgs args)
        {
            BoardCardCore attacker = (BoardCardCore)sender;
            if (!args.AttackedFields.Contains(field.BoardField))
            {
                if (attacker != field.ChildCard) field.Unhighlight();
                return;
            }     
            BoardCardCore defender = field.ChildCard;
            if (defender != null)
            {
                Vector2Int distanceToAttacker = defender.BoardCard.GetDistanceTo(attacker.BoardCard);
                if (defender.BoardCard.CharacterConfig.CanBlock(distanceToAttacker))
                {
                    field.HighlightAsUnderBlock();
                    return;
                }
                field.HighlightAsUnderAttack();
                if (defender.BoardCard.CharacterConfig.CanRiposte(distanceToAttacker)) attacker.ParentField.HighlightAsUnderAttack();
            }
            else field.HighlightAsUnderAttack();
        }

        private void HandleHighlightEnd()
        {
            field.Unhighlight();
        }
    }
}