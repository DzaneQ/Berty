using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.Enums;
using Berty.Gameplay.ConfigData;
using Berty.Grid.Entities;
using Berty.UI.Card.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

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

        public Status AddStatusWithNameAndProvider(StatusEnum name, BoardCard provider)
        {
            Status newStatus = new(name, provider);
            Statuses.Add(newStatus);
            return newStatus;
        }

        public Status IncrementChargedStatusWithNameAndAlignment(StatusEnum name, AlignmentEnum align, int delta)
        {
            Status status = Statuses.Find(x => x.Name == name && x.GetAlign() == align);
            if (status == null)
            {
                status = new(name, align, delta);
                Statuses.Add(status);
            }
            else status.IncrementCharges(delta);
            return status;
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

        public Status GetStatusByNameOrNull(StatusEnum name)
        {
            return Statuses.Find(x => x.Name == name);
        }

        public Status GetStatusByNameOrThrow(StatusEnum name)
        {
            return GetStatusByNameOrNull(name) ?? throw new Exception($"Unable to find status of name {name}");
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