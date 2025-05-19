using Berty.Characters.Data;
using Berty.ConfigData;
using Berty.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Entities
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
    }
}