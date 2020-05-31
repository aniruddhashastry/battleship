using System;
using System.Collections.Generic;
using System.Text;
using BattleshipStateTracker.Service.Implementations;
using BattleshipStateTracker.Service.Interfaces;
using System.Linq;
using BattleshipStateTracker.Service.CustomExceptions;
using BattleshipStateTracker.Service.Models;
using Moq;
using Xunit;

namespace BattleshipStateTracker.Service.Test
{
    public class BattleshipGameServiceTest
    {

        [Fact]
        public void CreatePlayer_Fails_When_2PlayersExists()
        {
            //Arrange
            List<string> players = new List<string> { "Alice", "Bob" };

            var mockDataStore = new Mock<IInMemoryDataStore>();
            mockDataStore.Setup(x => x.GetPlayers()).Returns(players);
            //mockDataStore.Setup(x => x.AddPlayer(It.IsAny<Player>()));

            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            var player = "John";

            var exception = Assert.Throws<BattleshipGameException>(() => battleshipGameService.CreatePlayer(player));

            Assert.Equal("It seems game is in progress as 2 players have already signed in. Please try after some time.", exception.Message);
        }


        [Fact]
        public void CreatePlayer_Fails_When_SameNameExists()
        {
            //Arrange
            List<string> players = new List<string> { "Alice" };

            var mockDataStore = new Mock<IInMemoryDataStore>();
            mockDataStore.Setup(x => x.GetPlayers()).Returns(players);
            //mockDataStore.Setup(x => x.AddPlayer(It.IsAny<Player>()));

            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            var player = "Alice";

            var exception = Assert.Throws<BattleshipGameException>(() => battleshipGameService.CreatePlayer(player));

            Assert.Equal("Player with this name is already registered for this game. Please try after some time.", exception.Message);

        }


        [Fact]
        public void CreatePlayer_Succeeds()
        {
            //Arrange
            List<string> players = new List<string> { "Alice" };

            var mockDataStore = new Mock<IInMemoryDataStore>();
            mockDataStore.Setup(x => x.GetPlayers()).Returns(players);
            mockDataStore.Setup(x => x.AddPlayer(It.IsAny<Player>()));

            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            var player = "Bob";

            battleshipGameService.CreatePlayer(player);

        }


        [Fact]
        public void CreateBoard_Fails_When_BoardExists()
        {
            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();
            mockDataStore.Setup(x => x.GetPlayer(It.IsAny<string>()))
                .Returns(()=>new Player("Alice") { Board = new Board(10)});

            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            var player = "Alice";
            int boardSize = 10;

            Assert.Throws<BattleshipGameException>(() => battleshipGameService.CreateBoard(player, boardSize));
        }

        [Fact]
        public void CreateBoard_Returns_True_When_BoardCreated()
        {
            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();
            mockDataStore.Setup(x => x.GetPlayer(It.IsAny<string>()))
                .Returns(() => new Player("Alice"));

            mockDataStore.Setup(x => x.UpdateBoard(It.IsAny<string>(), It.IsAny<Board>()))
                .Returns(() => true);

            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            var player = "Alice";
            int boardSize = 10;
            var result = battleshipGameService.CreateBoard(player,boardSize);
            Assert.True(result);

        }

        [Fact]
        public void CreateBoard_Returns_False_When_BoardCreationFailed()
        {
            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();
            mockDataStore.Setup(x => x.GetPlayer(It.IsAny<string>()))
                .Returns(() => new Player("Alice"));

            mockDataStore.Setup(x => x.UpdateBoard(It.IsAny<string>(), It.IsAny<Board>()))
                .Returns(() => false);

            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            var player = "Alice";
            int boardSize = 10;
            var result = battleshipGameService.CreateBoard(player, boardSize);
            Assert.False(result);

        }

        [Fact]
        public void AddBattleship_Fails_When_PlayerName_empty()
        {

            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();

            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "";
            Ship ship = new Ship(2);
            Coordinates startPosition = new Coordinates(1, 2);
            ShipPlacement shipPlacement = ShipPlacement.Row;

            
            var exception = Assert.Throws<BattleshipGameException>(() => battleshipGameService.AddBattleship(player, ship, startPosition, shipPlacement));

            Assert.Equal("player is a required field.", exception.Message);
        }


        [Fact]
        public void AddBattleship_Fails_When_Ship_isnull()
        {

            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();

            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "Alice";
            Ship ship = null;
            Coordinates startPosition = new Coordinates(1, 2);
            ShipPlacement shipPlacement = ShipPlacement.Row;


            var exception = Assert.Throws<BattleshipGameException>(() => battleshipGameService.AddBattleship(player, ship, startPosition, shipPlacement));

            Assert.Equal("Ship is a required field.", exception.Message);
        }


        [Fact]
        public void AddBattleship_Fails_When_Board_isnull()
        {

            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();
            mockDataStore.Setup(x => x.GetPlayer(It.IsAny<string>()))
                .Returns(() => new Player("Alice"));


            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "Alice";
            Ship ship = new Ship(2);
            Coordinates startPosition = new Coordinates(1, 2);
            ShipPlacement shipPlacement = ShipPlacement.Row;


            var exception = Assert.Throws<BattleshipGameException>(() => battleshipGameService.AddBattleship(player, ship, startPosition, shipPlacement));

            Assert.Equal("Alice does not have a game Board.", exception.Message);
        }


        [Fact]
        public void AddBattleship_Fails_When_Attack_Started()
        {
            var board = new Board(10) {AttackHasStarted = true};

            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();
            mockDataStore.Setup(x => x.GetPlayer(It.IsAny<string>()))
                .Returns(() => new Player("Alice") { Board = board });


            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "Alice";
            Ship ship = new Ship(2);
            Coordinates startPosition = new Coordinates(1, 2);
            ShipPlacement shipPlacement = ShipPlacement.Row;


            var exception = Assert.Throws<BattleshipGameException>(() => battleshipGameService.AddBattleship(player, ship, startPosition, shipPlacement));

            Assert.Equal("Attack has already started, you cannot add battleships now.", exception.Message);
        }



        [Fact]
        public void AddBattleship_Fails_When_Coordinates_OutOfBounds()
        {

            var board = new Board(10);

            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();
            mockDataStore.Setup(x => x.GetPlayer(It.IsAny<string>()))
                .Returns(() => new Player("Alice") { Board = board });


            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "Alice";
            Ship ship = new Ship(2);
            Coordinates startPosition = new Coordinates(11, 0);
            ShipPlacement shipPlacement = ShipPlacement.Row;


            var exception = Assert.Throws<BattleshipGameException>(() => battleshipGameService.AddBattleship(player, ship, startPosition, shipPlacement));

            Assert.Equal("Coordinate provided are out of range", exception.Message);
        }


        [Fact]
        public void AddBattleship_Fails_When_Ship_length_OutOfBounds()
        {

            var board = new Board(10);

            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();
            mockDataStore.Setup(x => x.GetPlayer(It.IsAny<string>()))
                .Returns(() => new Player("Alice") { Board = board });


            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "Alice";
            Ship ship = new Ship(11);
            Coordinates startPosition = new Coordinates(1, 0);
            ShipPlacement shipPlacement = ShipPlacement.Row;


            var exception = Assert.Throws<BattleshipGameException>(() => battleshipGameService.AddBattleship(player, ship, startPosition, shipPlacement));

            Assert.Equal("Ship length is not valid, minimum ship length is 1 and maximum ship length is 10", exception.Message);
        }



        [Fact]
        public void AddBattleship_Fails_When_Ship_OutOfBounds()
        {

            var board = new Board(10);

            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();
            mockDataStore.Setup(x => x.GetPlayer(It.IsAny<string>()))
                .Returns(() => new Player("Alice") { Board = board });


            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "Alice";
            Ship ship = new Ship(5);
            Coordinates startPosition = new Coordinates(1, 7);
            ShipPlacement shipPlacement = ShipPlacement.Row;


            var exception = Assert.Throws<BattleshipGameException>(() => battleshipGameService.AddBattleship(player, ship, startPosition, shipPlacement));

            Assert.Equal("Cannot add battleship at this position as the ship will go out of the board ", exception.Message);
        }


        [Fact]
        public void AddBattleship_Fails_When_Ship_Position_Not_Empty()
        {

            var board = new Board(10);
            var block = board.Blocks[1, 1];
            block.Occupant = new Ship(2);
            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();
            mockDataStore.Setup(x => x.GetPlayer(It.IsAny<string>()))
                .Returns(() => new Player("Alice") { Board = board });


            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "Alice";
            Ship ship = new Ship(5);
            Coordinates startPosition = new Coordinates(1, 1);
            ShipPlacement shipPlacement = ShipPlacement.Row;


            var exception = Assert.Throws<BattleshipGameException>(() => battleshipGameService.AddBattleship(player, ship, startPosition, shipPlacement));

            Assert.Equal("Cannot add battleship as the position is already occupied", exception.Message);
        }


        [Fact]
        public void AddBattleship_Ship_Placement_Update_Fails()
        {
            var board = new Board(10);
            var block = board.Blocks[1, 1];
            block.Occupant = new Ship(2);
            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();
            mockDataStore.Setup(x => x.GetPlayer(It.IsAny<string>()))
                .Returns(() => new Player("Alice") { Board = board });

            mockDataStore.Setup(x => x.UpdateBoard(It.IsAny<string>(), board))
                .Returns(() => false);


            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "Alice";
            Ship ship = new Ship(5);
            Coordinates startPosition = new Coordinates(1, 3);
            ShipPlacement shipPlacement = ShipPlacement.Row;


            var result = battleshipGameService.AddBattleship(player, ship, startPosition, shipPlacement);

            Assert.False(result);
        }


        [Fact]
        public void AddBattleship_Ship_Placement_Success()
        {
            var board = new Board(10);
            var block = board.Blocks[1, 1];
            block.Occupant = new Ship(2);
            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();
            mockDataStore.Setup(x => x.GetPlayer(It.IsAny<string>()))
                .Returns(() => new Player("Alice") { Board = board });

            mockDataStore.Setup(x => x.UpdateBoard(It.IsAny<string>(),board))
                .Returns(() => true);


            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "Alice";
            Ship ship = new Ship(5);
            Coordinates startPosition = new Coordinates(1, 3);
            ShipPlacement shipPlacement = ShipPlacement.Row;


            var result =  battleshipGameService.AddBattleship(player, ship, startPosition, shipPlacement);

            Assert.True(result);
        }

        [Fact]
        public void Attack_Fails_When_PlayerName_empty()
        {

            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();

            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "";
            Coordinates position = new Coordinates(1, 2);


            var exception = Assert.Throws<BattleshipGameException>(() => battleshipGameService.Attack(player, position));

            Assert.Equal("player is a required field.", exception.Message);
        }


        [Fact]
        public void Attack_Fails_When_Opponent_Not_Present()
        {

            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();

            List<string> players = new List<string> { "Alice" };
            mockDataStore.Setup(x => x.GetPlayers())
                .Returns(() => players);

            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "Bob";
            Coordinates position = new Coordinates(1, 2);


            var exception = Assert.Throws<BattleshipGameException>(() => battleshipGameService.Attack(player, position));

            Assert.Equal("Opponent is not ready! Wait for opponent to setup his board", exception.Message);
        }

        [Fact]
        public void Attack_Fails_When_OpponentBoard_Not_Present()
        {

            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();

            List<string> players = new List<string> { "Alice","Bob" };
            mockDataStore.Setup(x => x.GetPlayers())
                .Returns(() => players);

            mockDataStore.Setup(x => x.GetOpponentsBoard(It.IsAny<string>()))
                .Returns(() => null);

            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "Bob";
            Coordinates position = new Coordinates(1, 2);


            var exception = Assert.Throws<BattleshipGameException>(() => battleshipGameService.Attack(player, position));

            Assert.Equal("Opponent does not have a game Board.", exception.Message);
        }

        [Fact]
        public void Attack_Fails_When_Coordinates_OutOfBound()
        {

            var board = new Board(10);

            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();

            List<string> players = new List<string> { "Alice", "Bob" };
            mockDataStore.Setup(x => x.GetPlayers())
                .Returns(() => players);

            mockDataStore.Setup(x => x.GetOpponentsBoard(It.IsAny<string>()))
                .Returns(() => board);

            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "Bob";
            Coordinates position = new Coordinates(11, 2);


            var exception = Assert.Throws<BattleshipGameException>(() => battleshipGameService.Attack(player, position));

            Assert.Equal("Coordinate provided are out of range", exception.Message);
        }


        [Fact]
        public void Attack_Fails_When_Coordinates_Has_Already_Been_Attacked()
        {

            var board = new Board(10);
            var block = board.Blocks[1, 1];
            block.HasBeenAttacked = true;


            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();


            List<string> players = new List<string> { "Alice", "Bob" };
            mockDataStore.Setup(x => x.GetPlayers())
                .Returns(() => players);

            mockDataStore.Setup(x => x.GetOpponentsBoard(It.IsAny<string>()))
                .Returns(() => board);

            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "Bob";
            Coordinates position = new Coordinates(1, 1);


            var exception = Assert.Throws<BattleshipGameException>(() => battleshipGameService.Attack(player, position));

            Assert.Equal("Provided coordinates has already been attacked once.", exception.Message);
        }


        [Fact]
        public void Attack_Returns_Miss()
        {

            var board = new Board(10);
            var block = board.Blocks[1, 1];
            block.Occupant = new Ship(2);


            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();

            List<string> players = new List<string> { "Alice", "Bob" };
            mockDataStore.Setup(x => x.GetPlayers())
                .Returns(() => players);

            mockDataStore.Setup(x => x.GetOpponentsBoard(It.IsAny<string>()))
                .Returns(() => board);

            mockDataStore.Setup(x => x.UpdateBoard(It.IsAny<string>(), It.IsAny<Board>()))
                .Returns(() => true);

            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "Bob";
            Coordinates position = new Coordinates(2, 1);


            var result =  battleshipGameService.Attack(player, position);

            Assert.Equal(ShipState.Miss, result);
        }


        [Fact]
        public void Attack_Returns_Hit()
        {

            var board = new Board(10);
            var block = board.Blocks[1, 1];
            block.Occupant = new Ship(2);


            //Arrange
            var mockDataStore = new Mock<IInMemoryDataStore>();

            List<string> players = new List<string> { "Alice", "Bob" };
            mockDataStore.Setup(x => x.GetPlayers())
                .Returns(() => players);

            mockDataStore.Setup(x => x.GetOpponentsBoard(It.IsAny<string>()))
                .Returns(() => board);

            mockDataStore.Setup(x => x.UpdateBoard(It.IsAny<string>(), It.IsAny<Board>()))
                .Returns(() => true);

            var battleshipGameService = new BattleshipGameService(mockDataStore.Object);

            string player = "Bob";
            Coordinates position = new Coordinates(1, 1);


            var result = battleshipGameService.Attack(player, position);

            Assert.Equal(ShipState.Hit, result);
        }


    }
}
