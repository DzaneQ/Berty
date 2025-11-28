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
        public BoardCardCore Core { get; private set; }
        public BoardCardEntity Entity { get; private set; }
        public BoardCardNavigation Navigation { get; private set; }
        public BoardCardSound Sound { get; private set; }
        public BoardCardSprite Sprite { get; private set; }
        public BoardCardStateMachine StateMachine { get; private set; }
        protected Game game;
        public BoardCard BoardCard => Entity.BoardCard;
        public FieldBehaviour ParentField => Entity.ParentField;

        protected virtual void Awake()
        {
            Bars = GetComponent<BoardCardBarsObjects>();
            Core = GetComponent<BoardCardCore>();
            Navigation = GetComponent<BoardCardNavigation>();
            Sound = GetComponent<BoardCardSound>();
            Sprite = GetComponent<BoardCardSprite>();
            Entity = GetComponent<BoardCardEntity>();
            StateMachine = GetComponent<BoardCardStateMachine>();
            game = EntityLoadManager.Instance.Game;
        }
    }
}