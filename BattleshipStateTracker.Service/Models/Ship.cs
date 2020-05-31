using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipStateTracker.Service.Models
{
    public class Ship
    {
        public int Length { get; set; }
        public string Type => $"Ship-{Length}";
        public Guid Id { get; set; }

        public Ship(int length)
        {
            Id = Guid.NewGuid();
            Length = length;
        }
    }
    
}
