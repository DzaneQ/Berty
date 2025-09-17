using Berty.BoardCards.ConfigData;
using Berty.UI.Card.Entities;
using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Gameplay.ConfigData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Berty.BoardCards.Entities;

namespace Berty.Gameplay.Entities
{
    public class Game
    {
        private List<Status> Statuses { get; }
        public AlignmentEnum CurrentAlignment { get; private set; }
        public BoardGrid Grid { get; }
        public CardPile CardPile { get; }
        public GameConfig GameConfig { get; }


        public Game(AlignmentEnum startingAlignment)
        {
            Statuses = new();
            CurrentAlignment = startingAlignment;
            Grid = new BoardGrid(this);
            CardPile = new CardPile();
            GameConfig = new GameConfig();
        }

        public AlignmentEnum SwitchAlignment()
        {
            CurrentAlignment = CurrentAlignment switch
            {
                AlignmentEnum.Player => AlignmentEnum.Opponent,
                AlignmentEnum.Opponent => AlignmentEnum.Player,
                _ => throw new Exception("Invalid alignment to switch to"),
            };
            return CurrentAlignment;
        }

        public void AddStatusWithNameAndProvider(StatusEnum name, BoardCard provider)
        {
            Statuses.Add(new Status(name, provider));
        }

        public void AddStatusWithNameAndAlignment(StatusEnum name, AlignmentEnum align)
        {
            Statuses.Add(new Status(name, align));
        }

        public bool HasStatusByName(StatusEnum status)
        {
            return Statuses.Find(x => x.Name == status) != null;
        }

        public bool HasStatusByNameAndAlignment(StatusEnum status, AlignmentEnum align)
        {
            return Statuses.Find(x => x.Name == status && x.GetAlign() == align) != null;
        }

        public Status FindStatusFromProviderOrNull(BoardCard provider)
        {
            return Statuses.Find(x => x.Provider == provider);
        }

        public void RemoveStatusByName(StatusEnum status)
        {
            Statuses.Remove(Statuses.Find(x => x.Name == status));
        }
    }
}