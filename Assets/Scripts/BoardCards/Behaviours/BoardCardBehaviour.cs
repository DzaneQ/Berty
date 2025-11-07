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
        public BoardCardMovableObject Navigation { get; private set; }
        public BoardCardStatChange StatChange { get; private set; }
        public BoardCardStateMachine StateMachine { get; private set; }
        protected Game game;
        public BoardCard BoardCard => Core.BoardCard;
        public FieldBehaviour ParentField => Core.ParentField;

        protected virtual void Awake()
        {
            Bars = GetComponent<BoardCardBarsObjects>();
            Core = GetComponent<BoardCardCore>();
            Navigation = GetComponent<BoardCardMovableObject>();
            StatChange = GetComponent<BoardCardStatChange>();
            StateMachine = GetComponent<BoardCardStateMachine>();
            game = CoreManager.Instance.Game;
        }
    }
}