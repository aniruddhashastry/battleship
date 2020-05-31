using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipStateTracker.Service.Models
{
    //Every square unit is called a block
    public class Block
    {
        public Ship Occupant { get; set; }
        public bool HasBeenAttacked { get; set; }

    }
}
