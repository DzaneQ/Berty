using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Managers;
using Berty.BoardCards.State;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Grid.Field.Behaviour;
using Berty.Grid.Field.Entities;
using Berty.UI.Card;
using Berty.UI.Card.Managers;
using Berty.UI.Card.Systems;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardCore : MonoBehaviour
    {
        private Game Game;
        private CardStateEnum _cardState;
        private SpriteRenderer characterSprite;
        private Rigidbody cardRB;
        public BoardCardBarsObjects Bars { get; private set; }
        public BoardCardMovableObject CardNavigation { get; private set; }

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

        private void Awake()
        {
            BoardCardCollectionManager.Instance.AddCardToCollection(this);
            Game = CoreManager.Instance.Game;
            characterSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
            Bars = GetComponent<BoardCardBarsObjects>();
            CardNavigation = GetComponent<BoardCardMovableObject>();
            ParentField = GetComponentInParent<FieldBehaviour>();
            SelectionAndPaymentSystem system = CoreManager.Instance.SelectionAndPaymentSystem;
            BoardCard = ParentField.BoardField.AddCard(system.GetCardOnHoldOrThrow(), Game.CurrentAlignment);
        }

        private void Start()
        {
            UpdateCharacter();
            CardState = CardStateEnum.NewCard;
            InitializeRigidbody();
        }

        private void OnCollisionEnter(Collision collision)
        {
            ApplyPhysics(false);
        }

        public void HandleClick()
        {
            Debug.Log("Dummy message.");
        }

        private void UpdateCharacter()
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
            if (currentAlign == cardAlign) SetActive();
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

        public void SetTelecinetic()
        {
            CardState = CardStateEnum.Telekinetic;
        }

        public void SetAttacking()
        {
            CardState = CardStateEnum.Attacking;
        }

        public bool IsForPay()
        {
            if (CardState == CardStateEnum.NewCard) return true;
            if (CardState == CardStateEnum.NewTransform) return true;
            if (CardState == CardStateEnum.Attacking) return true;
            return false;
        }

        public bool IsDexterityBased()
        {
            if (CardState == CardStateEnum.Active) return true;
            if (CardState == CardStateEnum.Telekinetic) return true;
            return false;
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

        public void HandleAnimationEnd()
        {
            if (CardNavigation.IsCardAnimating()) return;
            if (!Bars.AreBarsAnimating()) CardNavigation.EnableInteraction();
            Bars.ShowBars();
        }

        public void KillCard()
        {
            Game.CardPile.MarkCardAsDead(BoardCard.CharacterConfig);
            DestroyCard();
        }

        public void DestroyCard()
        {
            BoardCard.DeactivateCard(); // TODO: Prove that the BoardCard entity no longer exists.
            ParentField.UpdateField();
            BoardCardCollectionManager.Instance.RemoveCardFromCollection(this);
            Destroy(gameObject);
        }
    }
}