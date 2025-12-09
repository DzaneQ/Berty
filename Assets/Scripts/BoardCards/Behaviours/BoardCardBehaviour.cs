using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public abstract class BoardCardBehaviour : MonoBehaviour
    {
        public BoardCardBarsObjects Bars { get; private set; }
        public BoardCardEntityHandler EntityHandler { get; private set; }
        public BoardCardNavigation Navigation { get; private set; }
        public BoardCardSound Sound { get; private set; }
        public BoardCardSprite Sprite { get; private set; }
        public BoardCardStateMachine StateMachine { get; private set; }
        protected Game game;
        public BoardCard BoardCard => EntityHandler.BoardCard;
        public FieldBehaviour ParentField => EntityHandler.ParentField;

        protected virtual void Awake()
        {
            Bars = GetComponent<BoardCardBarsObjects>();
            EntityHandler = GetComponent<BoardCardEntityHandler>();
            Navigation = GetComponent<BoardCardNavigation>();
            Sound = GetComponent<BoardCardSound>();
            Sprite = GetComponent<BoardCardSprite>();
            StateMachine = GetComponent<BoardCardStateMachine>();
            game = EntityLoadManager.Instance.Game;
        }

        public bool IsEqualTo(BoardCardBehaviour cardBehaviour)
        {
            return gameObject == cardBehaviour.gameObject;
        }
    }
}