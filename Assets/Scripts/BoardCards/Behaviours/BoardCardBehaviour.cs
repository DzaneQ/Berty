using Berty.BoardCards.Entities;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using System;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public abstract class BoardCardBehaviour : MonoBehaviour
    {
        public BoardCardActivation Activation { get; private set; }
        public BoardCardBarsObjects Bars { get; private set; }
        public BoardCardEntityHandler EntityHandler { get; private set; }
        public BoardCardNavigation Navigation { get; private set; }
        public BoardCardSound Sound { get; private set; } // nullable
        public BoardCardSprite Sprite { get; private set; }
        public BoardCardStateMachine StateMachine { get; private set; }
        protected Game game;
        public BoardCard BoardCard => EntityHandler.BoardCard;
        public FieldBehaviour ParentField => EntityHandler.ParentField;

        protected virtual void Awake()
        {
            Activation = GetBoardCardBehaviourComponentOrThrow<BoardCardActivation>();
            Bars = GetBoardCardBehaviourComponentOrThrow<BoardCardBarsObjects>();
            EntityHandler = GetBoardCardBehaviourComponentOrThrow<BoardCardEntityHandler>();
            Navigation = GetBoardCardBehaviourComponentOrThrow<BoardCardNavigation>();
            Sound = GetComponent<BoardCardSound>();
            Sprite = GetBoardCardBehaviourComponentOrThrow<BoardCardSprite>();
            StateMachine = GetBoardCardBehaviourComponentOrThrow<BoardCardStateMachine>();
            game = EntityLoadManager.Instance.Game;
        }

        public bool IsEqualTo(BoardCardBehaviour cardBehaviour)
        {
            return gameObject == cardBehaviour.gameObject;
        }

        private T GetBoardCardBehaviourComponentOrThrow<T>() where T : BoardCardBehaviour
        {
            T component = GetComponent<T>();
            return component == null
                ? throw new InvalidOperationException($"Component of type {typeof(T).Name} not found in {name}")
                : component;
        }
    }
}