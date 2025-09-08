using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Managers;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using System;
using UnityEngine;

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
            EventManager.Instance.OnValueChange += HandleValueChange;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnNewCharacter -= HandleNewCharacter;
            EventManager.Instance.OnMovedCharacter -= HandleMovedCharacter;
            EventManager.Instance.OnValueChange -= HandleValueChange;
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
            HandleMovedCardWitness(core, movedCharacter);
        }

        private void HandleValueChange(object sender, ValueChangeEventArgs args)
        {
            BoardCardCore sourceCharacter = (BoardCardCore)sender;
            HandleValueChange(core, sourceCharacter, args.Delta);
        }

        public void HandleNewCardWitness(BoardCardCore witness, BoardCardCore newCard)
        {
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
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(newCard, witness);
                    break;
                case CharacterEnum.ShaolinBert:
                    ApplySkillEffectManager.Instance.ApplyCharacterEffect(newCard, witness);
                    break;
            }
        }

        public void HandleMovedCardWitness(BoardCardCore witness, BoardCardCore movedCard)
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
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(witness, movedCard);
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
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(movedCard, witness);
                    break;
            }
        }

        public void HandleValueChange(BoardCardCore witness, BoardCardCore valueChangedCard, int delta)
        {
            // When value changed card is the character with skill
            switch (valueChangedCard.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.ZalobnyBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(witness, valueChangedCard, delta);
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