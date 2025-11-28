using Berty.BoardCards.Animation;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardNavigation : BoardCardBehaviour
    {
        private IMoveCard moveCard;
        private IRotateCard rotateCard;
        private bool queuedMovementEffect;
        private bool queuedMovementSkillEffect;

        protected override void Awake()
        {
            base.Awake();
            moveCard = GetComponent<IMoveCard>();
            rotateCard = GetComponent<IRotateCard>();
            queuedMovementEffect = false;
        }

        public void MoveCardObject(FieldBehaviour field)
        {
            moveCard.ToField(field);
            ParentField.UpdateField();
            EntityHandler.SetFieldBehaviour(field);
            if (StateMachine.HasState(CardStateEnum.Active) || StateMachine.HasState(CardStateEnum.Telekinetic)) return; // Don't run before NewTransform state is set
            HandleNewMovement();
        }

        public void RotateCardObject(int angle)
        {
            rotateCard.ByAngle(angle);
        }

        public void RotateObjectWithoutAnimation(int angle)
        {
            rotateCard.ByAngleWithoutAnimation(angle);
        }

        public bool IsCardAnimating()
        {
            return rotateCard.CoroutineCount > 0 || moveCard.CoroutineCount > 0;
        }

        public void HandleNewMovement()
        {
            queuedMovementEffect = true;
            if (!IsCardAnimating()) HandleAfterMoveAnimation();
        }

        public void HandleNewMovementSkillEffect()
        {
            if (IsCardAnimating()) queuedMovementSkillEffect = true;
            else EventManager.Instance.RaiseOnMovedCharacter(this);
        }

        public void HandleAfterMoveAnimation()
        {
            if (!queuedMovementEffect) return;
            ParentField.UpdateField();
            if (queuedMovementSkillEffect) EventManager.Instance.RaiseOnMovedCharacter(this);
            queuedMovementEffect = false;
        }

        public void HandleAnimationEnd()
        {
            if (BoardCard == null) return;
            if (Navigation.IsCardAnimating()) return;
            Bars.ShowBars();
            CheckpointManager.Instance.HandleIfRequested();
            if (Bars.AreBarsAnimating()) return;
            StateMachine.TryShowingButtons();
        }
    }
}