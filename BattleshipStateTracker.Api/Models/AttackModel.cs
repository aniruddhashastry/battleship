using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleshipStateTracker.Service.Models;

namespace BattleshipStateTracker.Api.Models
{
    public class AttackModel
    {
        public Coordinates Position { get; set; }
    }
}
