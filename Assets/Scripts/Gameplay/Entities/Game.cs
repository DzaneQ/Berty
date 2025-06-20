using Berty.BoardCards.ConfigData;
using Berty.CardTransfer.Entities;
using Berty.Entities;
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
        public Alignment CurrentAlignment { get; private set; }
        public BoardGrid Grid { get; }
        public CardPile CardPile { get; }
        public GameConfig GameConfig { get; }


        public Game(Alignment startingAlignment)
        {
            CurrentAlignment = startingAlignment;
            Grid = new BoardGrid();
            CardPile = new CardPile();
            GameConfig = new GameConfig();
        }

        public Alignment SwitchAlignment()
        {
            CurrentAlignment = CurrentAlignment switch
            {
                Alignment.Player => Alignment.Opponent,
                Alignment.Opponent => Alignment.Player,
                _ => throw new Exception("Invalid alignment to switch to"),
            };
            return CurrentAlignment;
        }
    }
}