using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleshipStateTracker.Service.Models;

namespace BattleshipStateTracker.Api.Models
{
    public class BattleshipModel
    {
        public Ship Ship { get; set; }

        public Coordinates StartPosition { get; set; }

        public ShipPlacement ShipPlacement { get; set; }
    }
}
