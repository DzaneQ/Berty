using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.Characters.Init;
using Berty.Enums;
using Berty.Gameplay.ConfigData;
using Berty.Grid.Entities;
using Berty.UI.Card.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public Game(GameSaveData data)
        {
            CharacterData characterData = new();
            List<CharacterConfig> allCharacters = characterData.LoadCharacterData();

            CurrentAlignment = data.CurrentAlignment;
            Grid = new BoardGrid(data.Grid, allCharacters, this);
            CardPile = new CardPile(data.CardPile, allCharacters);
            Statuses = new();
            foreach (StatusSaveData statusData in data.Statuses) Statuses.Add(new Status(statusData, Grid));
            GameConfig = new GameConfig();
        }

        public GameSaveData SaveEntity()
        {
            return new()
            {
                Statuses = this.Statuses.Select(status => status.SaveEntity()).ToArray(),
                CurrentAlignment = this.CurrentAlignment,
                Grid = this.Grid.SaveEntity(),
                CardPile = this.CardPile.SaveEntity(),
            };
            
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

        public Status AddStatusWithNameProviderAndTargetedAlign(StatusEnum name, BoardCard provider, AlignmentEnum targetAlignment)
        {
            Status newStatus = new(name, provider, targetAlignment);
            Statuses.Add(newStatus);
            return newStatus;
        }

        public Status AddStatusWithNameAndAlignment(StatusEnum name, AlignmentEnum alignment)
        {
            Status newStatus = new(name, alignment);
            Statuses.Add(newStatus);
            return newStatus;
        }

        public Status SetChargedStatusWithNameAndProvider(StatusEnum name, BoardCard provider, int charges)
        {
            Status status = Statuses.Find(x => x.Name == name && x.Provider == provider);
            if (status == null)
            {
                status = new(name, provider, charges);
                Statuses.Add(status);
            }
            else status.SetCharges(charges);
            return status;
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

        public List<CharacterConfig> GetAllCharacterConfigs()
        {
            return CardPile.GetAllCharactersOutsideField().Union(Grid.GetAllCharactersOnFields()).Distinct().ToList();
        }
    }

    [Serializable]
    public struct GameSaveData
    {
        public StatusSaveData[] Statuses;
        public AlignmentEnum CurrentAlignment;
        public BoardGridSaveData Grid;
        public CardPileSaveData CardPile;
    }
}