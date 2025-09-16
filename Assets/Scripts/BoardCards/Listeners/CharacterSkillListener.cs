using Berty.BoardCards.Animation;
using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Managers;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Entities;
using Berty.Structs;
using System;
using UnityEngine;
using UnityEngine.UIElements;

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
            EventManager.Instance.OnCharacterSpecialEffect += HandleCharacterSpecialEffect;
            EventManager.Instance.OnValueChange += HandleValueChange;
            EventManager.Instance.OnCharacterDeath += HandleCharacterDeath;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnNewCharacter -= HandleNewCharacter;
            EventManager.Instance.OnMovedCharacter -= HandleMovedCharacter;
            EventManager.Instance.OnValueChange -= HandleValueChange;
            EventManager.Instance.OnCharacterDeath -= HandleCharacterDeath;
        }

        private void HandleNewCharacter(object sender, EventArgs args)
        {
            BoardCardCore newCharacter = (BoardCardCore)sender;
            HandleNewCardWitness(core, newCharacter);
        }

        // BUG: This method is run even when cancelling move during NewTransform.
        private void HandleMovedCharacter(object sender, EventArgs args)
        {
            BoardCardCore movedCharacter = (BoardCardCore)sender;
            if (movedCharacter.BoardCard == null) return;
            HandleMovedCardWitness(core, movedCharacter);
        }

        private void HandleCharacterDeath(object sender, EventArgs args)
        {
            BoardCardCore dyingCharacter = (BoardCardCore)sender;
            HandleDeathWitness(core, dyingCharacter);
        }

        private void HandleCharacterSpecialEffect(object sender, EventArgs args)
        {
            BoardCardCore specialCharacter = (BoardCardCore)sender;
            HandleCustomEffect(core, specialCharacter);
        }

        private void HandleValueChange(object sender, ValueChangeEventArgs args)
        {
            BoardCardCore sourceCharacter = (BoardCardCore)sender;
            HandleCustomEffect(core, sourceCharacter, args.Delta);
        }

        private void HandleNewCardWitness(BoardCardCore witness, BoardCardCore newCard)
        {
            if (witness == newCard) HandleNewCardSelf(newCard);

            // When new card is the character with skill
            switch (newCard.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertaGejsza:
                case CharacterEnum.BertaSJW:
                case CharacterEnum.BertaTrojanska:
                case CharacterEnum.EBerta:
                case CharacterEnum.KuglarzBert:
                case CharacterEnum.MisiekBert:
                case CharacterEnum.PrymusBert:
                case CharacterEnum.SamurajBert:
                case CharacterEnum.SuperfanBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(witness, newCard);
                    break;
                case CharacterEnum.KonstablBert:
                case CharacterEnum.ShaolinBert:
                    ApplySkillEffectManager.Instance.ApplyCharacterEffect(witness, newCard);
                    break;
            }

            // When witness is the character with skill
            switch (witness.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertaGejsza:
                case CharacterEnum.BertaSJW:
                case CharacterEnum.KuglarzBert:
                case CharacterEnum.BertaTrojanska:
                case CharacterEnum.EBerta:
                case CharacterEnum.PrymusBert:
                case CharacterEnum.SamurajBert:
                case CharacterEnum.SuperfanBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(newCard, witness);
                    break;
                case CharacterEnum.ShaolinBert:
                    ApplySkillEffectManager.Instance.ApplyCharacterEffect(newCard, witness);
                    break;
            }
        }

        private void HandleMovedCardWitness(BoardCardCore witness, BoardCardCore movedCard)
        {
            if (witness == movedCard) HandleMovedCardSelf(movedCard);

            // When moved card is the character with skill
            switch (movedCard.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertaGejsza:
                case CharacterEnum.BertaSJW:
                case CharacterEnum.BertaTrojanska:
                case CharacterEnum.EBerta:
                case CharacterEnum.KuglarzBert:
                case CharacterEnum.MisiekBert:
                case CharacterEnum.PrezydentBert:
                case CharacterEnum.PrymusBert:
                case CharacterEnum.SamurajBert:
                case CharacterEnum.SuperfanBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(witness, movedCard);
                    break;
            }

            // When witness is the character with skill
            switch (witness.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertaGejsza:
                case CharacterEnum.BertaSJW:
                case CharacterEnum.BertaTrojanska:
                case CharacterEnum.EBerta:
                case CharacterEnum.KuglarzBert:
                case CharacterEnum.PrymusBert:
                case CharacterEnum.SamurajBert:
                case CharacterEnum.SuperfanBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(movedCard, witness);
                    break;
            }
        }

        private void HandleDeathWitness(BoardCardCore witness, BoardCardCore dyingCard)
        {
            if (witness == dyingCard) return;

            // When witness is the character with skill
            switch (witness.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.Zombert:
                    if (dyingCard.BoardCard.Align == witness.BoardCard.Align) break;
                    witness.StatChange.AdvanceHealth(1, null);
                    break;
            }
        }

        private void HandleCustomEffect(BoardCardCore witness, BoardCardCore customEffectCard, int delta = 0)
        {
            // When customed effect is triggered by the character with skill
            switch (customEffectCard.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.PapiezBertII:
                    ApplySkillEffectManager.Instance.ApplyCharacterEffect(witness, customEffectCard);
                    break;
                case CharacterEnum.KowbojBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(witness, customEffectCard, delta);
                    break;
                case CharacterEnum.ZalobnyBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(witness, customEffectCard, delta);
                    break;
            }
        }

        private void HandleNewCardSelf(BoardCardCore skillCard)
        {
            switch (skillCard.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.SuperfanBert:
                    int hour = DateTime.Now.Hour;
                    if (hour < 5 || 18 <= hour)
                    {
                        skillCard.StatChange.AdvanceStrength(1, null);
                        skillCard.StatChange.AdvancePower(2, null);
                    }
                    break;
            }
        }

        private void HandleMovedCardSelf(BoardCardCore skillCard)
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