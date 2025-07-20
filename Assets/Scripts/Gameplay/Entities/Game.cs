using Berty.BoardCards.ConfigData;
using Berty.UI.Card.Entities;
using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Gameplay.ConfigData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Gameplay.Entities
{
    public class Game
    {
        public AlignmentEnum CurrentAlignment { get; private set; }
        public BoardGrid Grid { get; }
        public CardPile CardPile { get; }
        public GameConfig GameConfig { get; }


        public Game(AlignmentEnum startingAlignment)
        {
            CurrentAlignment = startingAlignment;
            Grid = new BoardGrid();
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
    }
}