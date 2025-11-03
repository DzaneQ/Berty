using Berty.BoardCards.Entities;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public abstract class BoardCardBehaviour : MonoBehaviour
    {
        public BoardCardBarsObjects Bars { get; private set; }
        public BoardCardCore Core { get; private set; }
        public BoardCardMovableObject Navigation { get; private set; }
        public BoardCardStatChange StatChange { get; private set; }
        //public BoardCardState State { get; private set; }

        protected virtual void Awake()
        {
            Bars = GetComponent<BoardCardBarsObjects>();
            Core = GetComponent<BoardCardCore>();
            Navigation = GetComponent<BoardCardMovableObject>();
            StatChange = GetComponent<BoardCardStatChange>();
            //State = GetComponent<BoardCardState>();
        }
    }
}