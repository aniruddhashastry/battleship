using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipStateTracker.Service.CustomExceptions
{
    [Serializable]
    public class BattleshipGameException : Exception
    {
        public BattleshipGameException() { }

        public BattleshipGameException(string message)
            : base($"{message}")
        {

        }
    }
}
