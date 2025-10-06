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
using Berty.UI.Card.Managers;
using Berty.UI.Card.Systems;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardCore : MonoBehaviour
    {
        private Game Game;
        private CardStateEnum _cardState;
        private SpriteRenderer characterSprite;
        private Color defaultColor;
        private Rigidbody cardRB;
        private AudioSource soundSource;
        private List<BoardCardCore> _attackedCards = new List<BoardCardCore>();
        public BoardCardBarsObjects Bars { get; private set; }
        public BoardCardMovableObject CardNavigation { get; private set; }
        public BoardCardStatChange StatChange { get; private set; }
        public IReadOnlyList<BoardCardCore> AttackedCards => _attackedCards;

        public BoardCard BoardCard { get; private set; }
        public CardStateEnum CardState
        {
            get => _cardState;
            private set
            {
                if (_cardState == value) return;
                _cardState = value;
                CardNavigation.ActivateButtonsBasedOnState();
            }
        }
        public FieldBehaviour ParentField { get; private set; }
        public Sprite Sprite => characterSprite.sprite;
        public AudioSource SoundSource => soundSource;

        private void Awake()
        {
            BoardCardCollectionManager.Instance.AddCardToCollection(this);
            Game = CoreManager.Instance.Game;
            characterSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
            defaultColor = characterSprite.color;
            soundSource = GetComponent<AudioSource>();
            Bars = GetComponent<BoardCardBarsObjects>();
            CardNavigation = GetComponent<BoardCardMovableObject>();
            StatChange = GetComponent<BoardCardStatChange>();
            ParentField = GetComponentInParent<FieldBehaviour>();
            SelectionAndPaymentSystem system = CoreManager.Instance.SelectionAndPaymentSystem;
            BoardCard = ParentField.BoardField.AddNewCard(system.GetPendingCardOrThrow(), Game.CurrentAlignment);
        }

        private void Start()
        {
            SoundManager.Instance.PutSound(SoundSource);
            DisableBackupCard();
            AdjustInitRotation();
            UpdateObjectFromCharacterConfig();
            CardState = CardStateEnum.NewCard;
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

        public void SetMainState()
        {
            if (BoardCard == null || BoardCard.OccupiedField == null)
            {
                SetIdle();
                return;
            }
            AlignmentEnum currentAlign = Game.CurrentAlignment;
            AlignmentEnum cardAlign = BoardCard.Align;
            if (currentAlign == cardAlign && !BoardCard.IsTired) SetActive();
            else if (IsEligibleForTelekineticState()) SetTelekinetic();
            else SetIdle();
        }

        public void SetActive()
        {
            CardState = CardStateEnum.Active;
        }

        public void SetIdle()
        {
            CardState = CardStateEnum.Idle;
        }

        public void SetTelekinetic()
        {
            CardState = CardStateEnum.Telekinetic;
        }

        public void SetAttacking()
        {
            _attackedCards.Clear();
            CardState = CardStateEnum.Attacking;
        }

        public void SetEffectable()
        {
            CardState = CardStateEnum.Effectable;
        }

        public bool IsForPay()
        {
            if (CardState == CardStateEnum.NewCard) return true;
            if (CardState == CardStateEnum.NewTransform) return true;
            if (CardState == CardStateEnum.Attacking) return true;
            return false;
        }

        public bool IsEligibleForTelekineticState()
        {
            Status telekinesisArea = Game.GetStatusByNameOrNull(StatusEnum.TelekineticArea);
            if (telekinesisArea == null) return false;
            if (telekinesisArea.Provider.IsTired) return false;
            if (ApplySkillEffectManager.Instance.DoesPreventEffect(BoardCard, telekinesisArea.Provider)) return false;
            return telekinesisArea.Provider.Align == Game.CurrentAlignment;
        }

        public bool IsDexterityBased()
        {
            if (CardState == CardStateEnum.Active) return true;
            if (CardState == CardStateEnum.Telekinetic) return true;
            return false;
        }

        public bool IsOnNewMove()
        {
            if (CardState != CardStateEnum.NewTransform) return false;
            return CardNavigation.IsAnyMoveButtonActivated();
        }

        public void SetNewTransformFromNavigation(NavigationEnum navigation)
        {
            if (!IsDexterityBased()) return;
            NavigationEnum oppositeNavigation = navigation switch
            {
                NavigationEnum.MoveUp => NavigationEnum.MoveDown,
                NavigationEnum.MoveRight => NavigationEnum.MoveLeft,
                NavigationEnum.MoveDown => NavigationEnum.MoveUp,
                NavigationEnum.MoveLeft => NavigationEnum.MoveRight,
                NavigationEnum.RotateLeft => NavigationEnum.RotateRight,
                NavigationEnum.RotateRight => NavigationEnum.RotateLeft,
                _ => throw new ArgumentException("Invalid navigation for new transform.")
            };
            CardNavigation.ActivateNeutralButton(oppositeNavigation);
            CardState = CardStateEnum.NewTransform;
        }

        // WARNING: The code logic applies successful attack on riposte and attack new stand as it's basic attack.
        //    But it's only used when ordered attack is applied so it doesn't matter... until there's a new code that does.
        public void MarkSuccessfulAttack(BoardCardCore card)
        {
            _attackedCards.Add(card);
        }
        
        public void HandleAnimationEnd()
        {
            if (BoardCard == null) return;
            if (CardNavigation.IsCardAnimating()) return;
            if (!Bars.AreBarsAnimating()) CardNavigation.EnableInteraction();
            Bars.ShowBars();
            CheckpointManager.Instance.HandleIfRequested();
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
            CharacterConfig newCard = Game.CardPile.GetRandomKidFromPile();
            if (newCard == null)
            {
                KillCard();
                return;
            }
            StatusManager.Instance.RemoveStatusFromProvider(BoardCard);
            EventManager.Instance.RaiseOnCharacterDeath(this);
            DirectionEnum direction = (DirectionEnum)BoardCard.GetAngle();
            AlignmentEnum align = BoardCard.Align;
            Game.CardPile.MarkCardAsDead(BoardCard.CharacterConfig);
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
            if (BoardCard.GetSkill() == SkillEnum.BertWho) Game.CardPile.PutCardToTheBottomPile(BoardCard.CharacterConfig);
            else Game.CardPile.MarkCardAsDead(BoardCard.CharacterConfig);
            RemoveCard();
        }

        public void RemoveCard()
        {
            BoardCard.DeactivateCard(); // TODO: Prove that the BoardCard entity no longer exists.
            BoardCard = null;
            ParentField.UpdateField();
            BoardCardCollectionManager.Instance.RemoveCardFromCollection(this);
            if (transform.parent.childCount <= 1) Destroy(transform.parent.gameObject); // Remove CardSetTransform that has no cards
            else                                                                        // Otherwise, remove only the card object itself
            {
                EnableBackupCard();
                Destroy(gameObject);
            }    
        }

        public bool IsEligibleForCheckpoint()
        {
            if (CardNavigation.IsCardAnimating()) return false;
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
            CardNavigation.RotateObjectWithoutAnimation(rightAngle);
            BoardCard.AdvanceCardSetAngleBy(rightAngle);
        }
    }
}