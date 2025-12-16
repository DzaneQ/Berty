using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Managers;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using Berty.UI.Card.Managers;
using System;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardEntityHandler : BoardCardBehaviour
    {
        public new BoardCard BoardCard { get; private set; }
        public new FieldBehaviour ParentField { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            ParentField = GetComponentInParent<FieldBehaviour>();
        }

        public void LoadBoardCardEntity(CharacterConfig config, AlignmentEnum align)
        {
            BoardCard = ParentField.BoardField.AddNewCard(config, align);
            Sprite.UpdateObjectFromCharacterConfig();
            Bars.SetBars();
            ParentField.UpdateField();
        }

        public void DeactivateBoardCardEntity()
        {
            BoardCard.DeactivateCard();
            BoardCard = null;
            ParentField.UpdateField();
        }

        public void SetFieldBehaviour(FieldBehaviour fieldBehaviour)
        {
            ParentField = fieldBehaviour;
        }

        public void AdvanceStrength(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            if (ModifyStatChangeManager.Instance.BeforeStrengthChange(this, ref value, source)) return;
            BoardCard.AdvanceStrength(value);
            Bars.UpdateBar(StatEnum.Strength);
        }

        public void SetStrength(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            int delta = value - BoardCard.Stats.Strength;
            AdvanceStrength(delta, source);
        }

        public void AdvanceTempStrength(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            if (ModifyStatChangeManager.Instance.BeforeStrengthChange(this, ref value, source)) return;
            BoardCard.AdvanceTempStrength(value);
            Bars.UpdateBar(StatEnum.Strength);
        }

        public void SetTempStrength(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            int delta = value - BoardCard.Stats.TempStrength;
            AdvanceTempStrength(delta, source);
            Bars.UpdateBar(StatEnum.Strength);
        }

        public void AdvancePower(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            if (ModifyStatChangeManager.Instance.BeforePowerChange(this, ref value, source)) return;
            BoardCard.AdvancePower(value);
            Bars.UpdateBar(StatEnum.Power);
            ModifyStatChangeManager.Instance.AfterPowerChange(this, value, source);
        }

        public void SetPower(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            int delta = value - BoardCard.Stats.Power;
            AdvancePower(delta, source);
        }

        public void AdvanceTempPower(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            if (ModifyStatChangeManager.Instance.BeforePowerChange(this, ref value, source)) return;
            BoardCard.AdvanceTempPower(value);
            Bars.UpdateBar(StatEnum.Power);
            ModifyStatChangeManager.Instance.AfterPowerChange(this, value, source);
        }

        public void AdvanceDexterity(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            BoardCard.AdvanceDexterity(value);
            Bars.UpdateBar(StatEnum.Dexterity);
        }

        public void AdvanceHealth(int value, BoardCardBehaviour source, bool isBasicAttack = false)
        {
            if (BoardCard == null) return;
            if (ModifyStatChangeManager.Instance.BeforeHealthChange(this, ref value, source, isBasicAttack)) return;
            BoardCard.AdvanceHealth(value);
            Bars.UpdateBar(StatEnum.Health);
            ModifyStatChangeManager.Instance.AfterHealthChange(this, value, source);
        }

        public void ProgressTemporaryStats()
        {
            if (BoardCard.Stats.AreTempStatZeros()) return;
            BoardCard.Stats.ProgressTempStats();
            Bars.UpdateBars();
        }

        public void HandleAfterAnimationStatChange()
        {
            if (BoardCard.Stats.Health <= 0)
            {
                HandleZeroHealth();
            }
            if (BoardCard == null) return;
            if (BoardCard.Stats.Dexterity <= 0)
            {
                HandleZeroDexterity(); 
            }
            if (BoardCard == null) return;
            if (BoardCard.Stats.Dexterity >= BoardCard.CharacterConfig.Dexterity)
            {
                BoardCard.MarkAsRested();
            }
            if (BoardCard == null) return;
            if (BoardCard.Stats.Power <= 0)
            {
                HandleZeroPower();
            }    
        }

        public void SwitchSides()
        {
            BoardCard.OccupiedField.SwitchSides();
            EntityHandler.SetPower(BoardCard.CharacterConfig.Power, this);
            ParentField.UpdateField();
            EventManager.Instance.RaiseOnSideChanged(this);
        }

        private void KillCard()
        {
            TriggerCardDeath();
            Activation.DeactivateCard();
        }

        private void TriggerCardDeath()
        {
            StatusManager.Instance.RemoveStatusFromProvider(BoardCard);
            EventManager.Instance.RaiseOnCharacterDeath(this);
            if (BoardCard.GetSkill() == SkillEnum.BertWho) game.CardPile.PutCardToTheBottomPile(BoardCard.CharacterConfig);
            else game.CardPile.MarkCardAsDead(BoardCard.CharacterConfig);
        }

        // TODO: Refactor done. Check if color is persisted.
        private void UpdateCardWithRandomKid()
        {
            if (BoardCard.GetSkill() != SkillEnum.KrolPopuBert)
                throw new Exception($"KrolPopuBert effect is casted by {BoardCard.CharacterConfig.Name}");
            CharacterConfig newCard = game.CardPile.GetRandomKidFromPile();
            DirectionEnum direction = (DirectionEnum)BoardCard.GetAngle();
            AlignmentEnum align = BoardCard.Align;
            TriggerCardDeath();
            if (newCard == null) // If no kid in the deck, deactivate the card.
            {
                Activation.DeactivateCard();
                return;
            }
            BoardCard.DeactivateCard();
            LoadBoardCardEntity(newCard, align);
            BoardCard.SetDirection(direction);
            Bars.UpdateBars();
            EventManager.Instance.RaiseOnNewCharacter(this);
        }

        private void HandleZeroPower()
        {
            switch (BoardCard.GetSkill())
            {
                case SkillEnum.AstronautaBert:
                    KillCard();
                    break;
                case SkillEnum.KsiezniczkaBerta:
                    SetPower(BoardCard.CharacterConfig.Power, this);
                    break;
                default:
                    SwitchSides();
                    break;
            }
        }

        private void HandleZeroDexterity()
        {
            switch (BoardCard.GetSkill())
            {
                case SkillEnum.BertWick:
                    KillCard();
                    break;
                default:
                    BoardCard.MarkAsTired();
                    break;
            }
        }

        private void HandleZeroHealth()
        {
            switch (BoardCard.GetSkill())
            {
                case SkillEnum.BertWick:
                    AdvanceHealth(2, null);
                    AdvancePower(1, null);
                    AdvanceStrength(1, null);
                    AdvanceDexterity(-1, null);
                    break;
                case SkillEnum.KrolPopuBert:
                    UpdateCardWithRandomKid();
                    break;
                default:
                    KillCard();
                    break;
            }
        }

        private void EnableBackupCard()
        {
            transform.parent.GetChild(0).gameObject.SetActive(true);
        }
    }
}