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

        public void IncrementChargedStatusWithNameAndAlignment(StatusEnum name, AlignmentEnum align, int delta)
        {
            Status status = Statuses.Find(x => x.Name == name && x.GetAlign() == align);
            if (status == null) Statuses.Add(new Status(name, align, delta));
            else status.IncrementCharges(delta);
        }

        public bool HasStatusByName(StatusEnum name)
        {
            return Statuses.Find(x => x.Name == name) != null;
        }

        public bool HasStatusByNameAndAlignment(StatusEnum name, AlignmentEnum align)
        {
            return Statuses.Find(x => x.Name == name && x.GetAlign() == align) != null;
        }

        public bool HasStatusByNameOpposedToAlignment(StatusEnum name, AlignmentEnum align)
        {
            return Statuses.Find(x => x.Name == name && x.GetAlign() != align) != null;
        }

        public Status GetStatusByNameAndAlignmentOrNull(StatusEnum name, AlignmentEnum align)
        {
            return Statuses.Find(x => x.Name == name && x.GetAlign() == align);
        }

        public Status GetStatusFromProviderOrNull(BoardCard provider)
        {
            return Statuses.Find(x => x.Provider == provider);
        }

        public void RemoveStatusByName(StatusEnum name)
        {
            Statuses.Remove(Statuses.Find(x => x.Name == name));
        }

        public void RemoveStatus(Status status)
        {
            Statuses.Remove(status);
        }
    }
}