using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleshipStateTracker.Service.CustomExceptions;
using BattleshipStateTracker.Service.Interfaces;
using BattleshipStateTracker.Service.Models;

namespace BattleshipStateTracker.Service.Implementations
{
    public class BattleshipGameService : IBattleshipGameService
    {
        private readonly IInMemoryDataStore _inMemoryDataStore;

        public BattleshipGameService(IInMemoryDataStore inMemoryDataStore)
        {
            _inMemoryDataStore = inMemoryDataStore;
        }   


        public void CreatePlayer(string playerName)
        {
            var players = _inMemoryDataStore.GetPlayers();

            IsGameInProgress(players);

            IsPlayerAlreadyRegistered(players, playerName);

            var player = new Player(playerName);
            _inMemoryDataStore.AddPlayer(player);
        }

        public bool CreateBoard(string playerName, int boardSize)
        {
            var player = _inMemoryDataStore.GetPlayer(playerName);

            DoesPlayerExist(player);

            DoesBoardExist(player.Board);

            player.Board = new Board(boardSize);
            var result = _inMemoryDataStore.UpdateBoard(playerName, player.Board);
            return result;
        }

        public bool AddBattleship(string playerName, Ship ship, Coordinates startPosition, ShipPlacement shipPlacement)
        {
            ValidatePlayer(playerName);

            IsShipProvided(ship);

            var player = _inMemoryDataStore.GetPlayer(playerName);

            DoesPlayerExist(player);

            var board = player.Board;
            DoesBoardExist(playerName, board);
           

            if (board.AttackHasStarted)
            {
                throw new 
                    BattleshipGameException("Attack has already started, you cannot add battleships now.");
            }


            var boardSize = board.Blocks.GetLength(0);

            ValidateCoordinates(startPosition, boardSize);

            ValidateShipLength(ship, boardSize);

            if (shipPlacement == ShipPlacement.Row)
            {
                //row is fixed
                int row = startPosition.Row;

                if ((startPosition.Column + ship.Length) > boardSize)
                {
                    throw new BattleshipGameException($"Cannot add battleship at this position " +
                                                      $"as the ship will go out of the board ");
                }

                for (int col = startPosition.Column; col < startPosition.Column + ship.Length; col++)
                {
                    var block = board.Blocks[row, col];
                    ValidateIsBlockOccupied(block);
                }


                for (int col = startPosition.Column; col < startPosition.Column + ship.Length; col++)
                {
                    var block = board.Blocks[row, col];
                    block.Occupant = ship;
                }

            }
            else
            {
                //col is fixed
                int col = startPosition.Column;


                if ((startPosition.Row + ship.Length) > boardSize)
                {
                    throw new BattleshipGameException($"Cannot add battleship at this position " +
                                                      $"as the ship will go out of the board ");
                }


                for (int row = startPosition.Row; row < startPosition.Row + ship.Length; row++)
                {
                    var block = board.Blocks[row, col];
                    ValidateIsBlockOccupied(block);
                }


                for (int row = startPosition.Row; row < startPosition.Row + ship.Length; row++)
                {
                    var block = board.Blocks[row, col];
                    block.Occupant = ship;
                }

            }

            return _inMemoryDataStore.UpdateBoard(playerName, board);

        }



        public ShipState Attack(string playerName, Coordinates position)
        {
            ValidatePlayer(playerName);

            IsOpponentReady();

            var board = _inMemoryDataStore.GetOpponentsBoard(playerName);

            DoesBoardExist("Opponent", board);

            var boardSize = board.Blocks.GetLength(0);
            ValidateCoordinates(position, boardSize);

            var block = board.Blocks[position.Row, position.Column];

            ValidateBlockHasBeenAttacked(block);

            board.AttackHasStarted = true;
            block.HasBeenAttacked = true;

            if (_inMemoryDataStore.UpdateBoard(playerName, board))
            {
                if (block.Occupant == null)
                {
                    return ShipState.Miss;
                }
                else
                {

                    return ShipState.Hit;
                }
            }
            else
            {
                throw new BattleshipGameException("An error occured while processing the Attack information. Please try again.");
            }

        }

        private void IsOpponentReady()
        {
            var players = _inMemoryDataStore.GetPlayers();
            if (players.Count < 2)
            {
                throw new BattleshipGameException("Opponent is not ready! Wait for " +
                                                  "opponent to setup his board");
            }
        }

        public void ResetGame()
        {
            _inMemoryDataStore.Reset();
        }


        //Should be moved to its own class
        #region Validation helpers

        private static void ValidateBlockHasBeenAttacked(Block block)
        {
            if (block.HasBeenAttacked)
            {
                throw new BattleshipGameException("Provided coordinates has " +
                                                  "already been attacked once.");
            }
        }
        private static void ValidateIsBlockOccupied(Block block)
        {
            if (block.Occupant != null)
            {
                throw new BattleshipGameException($"Cannot add battleship as the position " +
                                                  $"is already occupied");
            }
        }

        private static bool IsRangeValid(int value, int minimum, int maximum)
        {
            return value >= minimum && value < maximum;
        }

        private static void ValidatePlayer(string playerName)
        {
            if (string.IsNullOrEmpty(playerName))
            {
                throw new BattleshipGameException("player is a required field.");
            }
        }


        private static void ValidateShipLength(Ship ship, int boardSize)
        {
            if (ship.Length > boardSize || ship.Length <= 0)
            {
                throw new BattleshipGameException($"Ship length is not valid, minimum ship length is 1 " +
                                                  $"and maximum ship length is {boardSize}");
            }
        }

        private static void ValidateCoordinates(Coordinates startPosition, int boardSize)
        {
            if (!IsRangeValid(startPosition.Row, 0, boardSize) ||
                !IsRangeValid(startPosition.Column, 0, boardSize))
            {
                throw new BattleshipGameException($"Coordinate provided are out of range");
            }
        }

        private static void DoesBoardExist(string userName, Board board)
        {
            if (board == null)
            {
                throw new BattleshipGameException($"{userName} does not have a game Board.");
            }
        }

        private static void IsShipProvided(Ship ship)
        {
            if (ship == null)
            {
                throw new BattleshipGameException("Ship is a required field.");
            }
        }

        private static void DoesPlayerExist(Player player)
        {
            if (player == null)
            {
                throw new BattleshipGameException("Player does not exist.");
            }
        }

        private static void DoesBoardExist(Board board)
        {
            if (board != null)
            {
                throw new BattleshipGameException("Board is already created");
            }
        }

        private static void IsGameInProgress(List<string> players)
        {
            if (players.Count == 2)
            {
                throw new
                    BattleshipGameException(
                    "It seems game is in progress as 2 players have already signed in. " +
                    "Please try after some time.");
            }
        }

        private static void IsPlayerAlreadyRegistered(List<string> players, string playerName)
        {
            if (players.Any(s => s.Equals(playerName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new
                     BattleshipGameException(
                    "Player with this name is already registered for this game. Please try after some time.");

            }
        }

        #endregion
    }
}
