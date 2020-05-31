using System;
using System.Collections.Generic;
using System.Text;
using BattleshipStateTracker.Service.Models;

namespace BattleshipStateTracker.Service.Interfaces
{
    public interface IInMemoryDataStore
    {
        List<string> GetPlayers();

        void AddPlayer(Player player);

        Player GetPlayer(string playerName);

        Board GetOpponentsBoard(string player);

        bool UpdateBoard(string player, Board board);

        void Reset();
    }
}
