using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipStateTracker.Service.Models
{
    public class Player
    {
        public string Name { get;set;}
        public Board Board { get; set; }

        public Player(string name)
        {
            Name = name;
        }

    }
}
