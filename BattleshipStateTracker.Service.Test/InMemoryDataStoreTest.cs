using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using BattleshipStateTracker.Service.CustomExceptions;
using BattleshipStateTracker.Service.Implementations;
using BattleshipStateTracker.Service.Models;
using Xunit;

namespace BattleshipStateTracker.Service.Test
{
    public class InMemoryDataStoreTest
    {

        [Fact]
        public void GetPlayers_Returns_EmptyList()
        {
            var inMemoryDataStore = new InMemoryDataStore();

            var players = inMemoryDataStore.GetPlayers();

            Assert.Empty(players);
        }


        [Fact]
        public void GetPlayers_Returns_SinglePlayer()
        {
            var inMemoryDataStore = new InMemoryDataStore();

            var player1 = new Player("Alice");
            inMemoryDataStore.AddPlayer(player1);

            var players = inMemoryDataStore.GetPlayers();

            Assert.Single(players);
        }


        [Fact]
        public void GetPlayers_Returns_TwoPlayer()
        {
            var inMemoryDataStore = new InMemoryDataStore();

            var player1 = new Player("Alice");
            var player2 = new Player("Bobgamer");
            inMemoryDataStore.AddPlayer(player1);
            inMemoryDataStore.AddPlayer(player2);

            var players = inMemoryDataStore.GetPlayers();

            Assert.Equal(2, players.Count());
        }

        [Fact]
        public void GetPlayer_Returns_Player()
        {
            var inMemoryDataStore = new InMemoryDataStore();

            var player = new Player("Alice")
            {
                Board = new Board(10)
            };

            inMemoryDataStore.AddPlayer(player);

            var output = inMemoryDataStore.GetPlayer("Alice");

            Assert.NotNull(output);

            Assert.Equal("Alice",output.Name);
        }

        [Fact]
        public void GetPlayer_Returns_Null()
        {
            var inMemoryDataStore = new InMemoryDataStore();   

            var output = inMemoryDataStore.GetPlayer("Alice");

            Assert.Null(output);
        }



        [Fact]
        public void AddPlayer_Success()
        {
            var inMemoryDataStore = new InMemoryDataStore();

            var player = new Player("Alice")
            {
                Board = new Board(10)
            };

            inMemoryDataStore.AddPlayer(player);

            var output = inMemoryDataStore.GetPlayer("Alice");

            Assert.NotNull(output);
            Assert.Equal("Alice", output.Name);

        }

        [Fact]
        public void GetOpponentsBoard_Returns_board()
        {
            var inMemoryDataStore = new InMemoryDataStore();

            var player1 = new Player("Alice")
            {
                Board = new Board(10)
            };

            var player2 = new Player("Bobgamer")
            {
                Board = new Board(10)
            };

            inMemoryDataStore.AddPlayer(player1);
            inMemoryDataStore.AddPlayer(player2);

            var board = inMemoryDataStore.GetOpponentsBoard("Alice");

            Assert.NotNull(board);
            
        }


        [Fact]
        public void GetOpponentsBoard_Returns_null_When_NoOpponentPresent()
        {
            var inMemoryDataStore = new InMemoryDataStore();

            var player1 = new Player("Alice")
            {
                Board = new Board(10)
            };

            inMemoryDataStore.AddPlayer(player1);

            var board = inMemoryDataStore.GetOpponentsBoard("Alice");

            Assert.Null(board);

        }


        [Fact]
        public void UpdateBoard_Returns_True()
        {
            var inMemoryDataStore = new InMemoryDataStore();

            var player1 = new Player("Alice")
            {
                Board = new Board(10)
            };

            inMemoryDataStore.AddPlayer(player1);

            var board = inMemoryDataStore.GetPlayer("Alice").Board;
            board.AttackHasStarted = true;

            var result = inMemoryDataStore.UpdateBoard("Alice", board);

            var updatedBoard = inMemoryDataStore.GetPlayer("Alice").Board;

            Assert.True(result);
            Assert.Equal(board.AttackHasStarted, updatedBoard.AttackHasStarted);

        }



        [Fact]
        public void UpdateBoard_Returns_False()
        {
            var inMemoryDataStore = new InMemoryDataStore();

            var player1 = new Player("Alice")
            {
                Board = new Board(10)
            };

            inMemoryDataStore.AddPlayer(player1);

            var board = inMemoryDataStore.GetPlayer("Alice").Board;
            board.AttackHasStarted = true;

            var result = inMemoryDataStore.UpdateBoard("Alice1", board);

            Assert.False(result);

        }

    }
}
