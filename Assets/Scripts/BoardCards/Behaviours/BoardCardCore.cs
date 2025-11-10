using Berty.Audio.Managers;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Managers;
using Berty.Characters.Managers;
using Berty.Display.View;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using Berty.Grid.Managers;
using Berty.Settings;
using Berty.UI.Card.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardCore : BoardCardBehaviour
    {
        public new BoardCard BoardCard { get; private set; }
        public new FieldBehaviour ParentField { get; private set; }
        private SpriteRenderer characterSprite;
        private Color defaultColor;
        private Rigidbody cardRB;
        private AudioSource soundSource;
        private List<BoardCardBehaviour> _attackedCards = new List<BoardCardBehaviour>();
        public IReadOnlyList<BoardCardBehaviour> AttackedCards => _attackedCards;
        public Sprite Sprite => characterSprite.sprite;
        public AudioSource SoundSource => soundSource;
        private Camera cam;

        protected override void Awake()
        {
            base.Awake();
            BoardCardCollectionManager.Instance.AddCardToCollection(this);
            characterSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
            defaultColor = characterSprite.color;
            soundSource = GetComponent<AudioSource>();
            soundSource.volume = SettingsManager.Instance.Volume;
            ParentField = GetComponentInParent<FieldBehaviour>();
            BoardCard = ParentField.BoardField.AddNewCard(SelectionManager.Instance.GetPendingCardOrThrow(), game.CurrentAlignment);
            cam = Camera.main;
        }

        private void Start()
        {
            SoundManager.Instance.PutSound(SoundSource);
            DisableBackupCard();
            AdjustInitRotation();
            UpdateObjectFromCharacterConfig();
            ParentField.UpdateField();
            InitializeRigidbody();
        }

        private void OnCollisionEnter(Collision collision)
        {
            ApplyPhysics(false);
        }

        private void UpdateObjectFromCharacterConfig()
        {
            CharacterConfig character = BoardCard.CharacterConfig;
            characterSprite.sprite = HandCardObjectManager.Instance.GetSpriteFromHandCardObject(character);
            gameObject.name = character.Name;
        }

        private void InitializeRigidbody()
        {
            cardRB = GetComponent<Rigidbody>();
            cardRB.detectCollisions = true;
            ApplyPhysics(true);
        }

        public void ApplyPhysics(bool isApplied = true)
        {
            cardRB.isKinematic = !isApplied;
        }

        public void HighlightAs(HighlightEnum highlight)
        {
            characterSprite.color = highlight switch { 
                HighlightEnum.UnderAttack or HighlightEnum.UnderBlock => ColorizeObjectManager.Instance.GetColorForCard(highlight),
                _ => defaultColor
            };
        }

        public void SetFieldBehaviour(FieldBehaviour fieldBehaviour)
        {
            ParentField = fieldBehaviour;
        }

        public void ClearAttackedCardsCache()
        {
            _attackedCards.Clear();
        }

        // WARNING: The code logic applies successful attack on riposte and attack new stand as it's basic attack.
        //    But it's only used when ordered attack is applied so it doesn't matter... until there's a new code that does.
        public void MarkSuccessfulAttack(BoardCardBehaviour card)
        {
            _attackedCards.Add(card);
        }

        public bool IsCursorFocused()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit)) return false;
            if (hit.transform == transform) return true; // Is cursor on card square object?
            if (hit.transform.parent == null) return false;
            return hit.transform.parent.parent == transform; // Is cursor on card's button object?
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

        // TODO: Handle changed side for cards that apply skills to allies
        public void SwitchSides()
        {
            BoardCard.OccupiedField.SwitchSides();
            StatChange.SetPower(BoardCard.CharacterConfig.Power, this);
            ParentField.UpdateField();
        }

        public void UpdateCardWithRandomKid()
        {
            if (BoardCard.GetSkill() != SkillEnum.KrolPopuBert)
                throw new Exception($"KrolPopuBert effect is casted by {BoardCard.CharacterConfig.Name}");
            CharacterConfig newCard = game.CardPile.GetRandomKidFromPile();
            if (newCard == null)
            {
                KillCard();
                return;
            }
            StatusManager.Instance.RemoveStatusFromProvider(BoardCard);
            EventManager.Instance.RaiseOnCharacterDeath(this);
            DirectionEnum direction = (DirectionEnum)BoardCard.GetAngle();
            AlignmentEnum align = BoardCard.Align;
            game.CardPile.MarkCardAsDead(BoardCard.CharacterConfig);
            BoardCard.DeactivateCard();
            BoardCard = ParentField.BoardField.AddNewCard(newCard, align);
            BoardCard.SetDirection(direction);
            UpdateObjectFromCharacterConfig();
            Bars.UpdateBars();
            characterSprite.color = defaultColor;
            ParentField.UpdateField();
            EventManager.Instance.RaiseOnNewCharacter(this);
        }

        public void KillCard()
        {
            StatusManager.Instance.RemoveStatusFromProvider(BoardCard);
            EventManager.Instance.RaiseOnCharacterDeath(this);      
            if (BoardCard.GetSkill() == SkillEnum.BertWho) game.CardPile.PutCardToTheBottomPile(BoardCard.CharacterConfig);
            else game.CardPile.MarkCardAsDead(BoardCard.CharacterConfig);
            RemoveCard();
        }

        public void RemoveCard()
        {
            BoardCard.DeactivateCard(); // TODO: Prove that the BoardCard entity no longer exists.
            BoardCard = null;
            ParentField.UpdateField();
            BoardCardCollectionManager.Instance.RemoveCardFromCollection(this);
            if (transform.parent.childCount <= 1)  // Remove CardSetTransform that has no cards
            {
                EventManager.Instance.RaiseOnFieldFreed(ParentField);
                Destroy(transform.parent.gameObject);
            }
            else                                   // Otherwise, remove only the card object itself
            {
                EnableBackupCard();
                Destroy(gameObject);
            }    
        }

        public bool IsEligibleForCheckpoint()
        {
            if (Navigation.IsCardAnimating()) return false;
            if (BoardCard.Stats.Health <= 0) return false;
            if (BoardCard.Stats.Power <= 0) return false;
            if (BoardCard.GetSkill() == SkillEnum.BertWick && BoardCard.Stats.Dexterity <= 0) return false;
            return true;
        }

        private void EnableBackupCard()
        {
            transform.parent.GetChild(0).gameObject.SetActive(true);
        }

        private void DisableBackupCard()
        {
            GameObject backupCard = transform.parent.GetChild(0).gameObject;
            if (backupCard == gameObject) return;
            backupCard.SetActive(false);
        }
        
        private void AdjustInitRotation()
        {
            if (transform.parent.childCount > 1) return; // Keep the backup card's rotation.
            int rightAngle = (180 - Mathf.RoundToInt(Camera.main.GetComponent<RotateCamera>().RightAngleValue())) % 360;
            Navigation.RotateObjectWithoutAnimation(rightAngle);
            BoardCard.AdvanceCardSetAngleBy(rightAngle);
        }
    }
}