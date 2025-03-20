﻿using Berty.CardSprite;
using Berty.Enums;
using Berty.Field;
using Berty.UI.Card;

namespace Berty.Characters.Data
{
    public class KrolPopuBert : Character
    {
        public KrolPopuBert()
        {
            AddName("krol popu bert");
            AddProperties(Gender.Male, Role.Special);
            AddStats(1, 3, 5, 2);
            AddRange(0, 1, attackRange);
            AddRange(1, 2, attackRange);
            AddRange(-1, 2, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("570804__soundslikewillem__orchestra-hit-2");
        }

        public override void SkillAdjustHealthChange(int value, CardSpriteBehaviour card) // BUG: glitchy interaction with cards that move on attack (serferka berta, ronin bert)
        {
            if (card.CardStatus.Health > 0) return;
            CardImage kid = null;
            foreach (CardImage image in card.CardManager.AllOutsideCards())
            {
                if (image.Character.Gender != Gender.Kid) continue;
                if (IsEnemyCard(image.Character, card)) continue;
                kid = image;
                break;
            }
            if (kid == null) return;
            card.CardManager.RemoveCharacter(kid.Character);
            foreach (FieldBehaviour field in card.Grid.Fields)
                if (field.IsOccupied() && field.OccupantCard.CanUseSkill())
                    field.OccupantCard.Character.SkillOnOtherCardDeath(field.OccupantCard, card);
            card.UpdateCard(kid);
            if (card.CanUseSkill()) card.Character.SkillOnNewCard(card);
        }

        private bool IsEnemyCard(Character character, CardSpriteBehaviour card)
        {
            if (card.Grid.Turn.CurrentAlignment == card.OccupiedField.Align)
                return card.CardManager.DisabledCards.FindIndex(x => x.Character.GetType() == character.GetType()) >= 0;
            else
                return card.CardManager.EnabledCards.FindIndex(x => x.Character.GetType() == character.GetType()) >= 0;
        }
    }
}