using System;
using System.Collections.Generic;
using System.Linq;
using BattleshipStateTracker.Service.Interfaces;
using BattleshipStateTracker.Service.Models;

namespace BattleshipStateTracker.Service.Implementations
{
    public class InMemoryDataStore : IInMemoryDataStore
    {
        private List<Player> _stateTracker;

        public InMemoryDataStore()
        {
            _stateTracker = new List<Player>();
        }

        public List<string> GetPlayers()
        {
            return _stateTracker.Select(x => x.Name).ToList();
        }


        public void AddPlayer(Player player)
        {
            _stateTracker.Add(player);  
        }

        public Player GetPlayer(string playerName)
        {
            return _stateTracker.SingleOrDefault(x => x.Name == playerName);

        }

        public Board GetOpponentsBoard(string player)
        {
            var opponent = _stateTracker.SingleOrDefault(x => x.Name != player);
            if (opponent == null)
            {
                return null;
            }
            var board = opponent.Board;
            return board;
        }

        public bool UpdateBoard(string playerName, Board board)
        {
            var result = true;
            try
            {
                var player = _stateTracker.Single(x => x.Name == playerName);
                player.Board = board;
            }
            catch (Exception)
            {
                //We can add exceptions to the log file here
                result = false;
            }
            return result;
        }

        public void Reset()
        {
            _stateTracker.Clear();
        }
    }
}
