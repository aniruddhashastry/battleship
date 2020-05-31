using System.Collections.Generic;
using System.Threading.Tasks;
using BattleshipStateTracker.Service.Models;

namespace BattleshipStateTracker.Service.Interfaces
{
    public interface IBattleshipGameService
    {
        void CreatePlayer(string playerName);
        bool CreateBoard(string playerName, int boardSize);
        bool AddBattleship(string playerName, Ship ship, Coordinates startPosition, ShipPlacement shipPlacement);
        ShipState Attack(string playerName, Coordinates position);
        void ResetGame();
    }
}
