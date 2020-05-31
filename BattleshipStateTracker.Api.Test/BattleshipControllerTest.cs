using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using BattleshipStateTracker.Api.Controllers;
using BattleshipStateTracker.Api.Models;
using BattleshipStateTracker.Api.Models.Response;
using BattleshipStateTracker.Service.CustomExceptions;
using BattleshipStateTracker.Service.Interfaces;
using BattleshipStateTracker.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace BattleshipStateTracker.Api.Test
{
    public class BattleshipControllerTest
    {

        private readonly IOptions<BattleshipGameSettings> _config;

        public BattleshipControllerTest()
        {
            _config = Options.Create(new BattleshipGameSettings
            {
                BoardSize = 10
            });
        }

        //[Fact]
        //public void CreatePlayer_Success()
        //{
        //    var controller = ControllerCreatePlayer();

        //    var result = controller.CreatePlayer();
        //    var apiResponse = (ApiResponse<string>)result;


        //    //Assert
        //    Assert.Equal(HttpStatusCode.Created, apiResponse.StatusCode);
        //}



        //[Fact]
        //public void CreatePlayer_Fails_For_3rd_Player()
        //{

        //    var user = MockClaimsPrincipal();

        //    var mockService = new Mock<IBattleshipGameService>();

        //    var playerList = new List<string> {"alice", "bob"};

        //    mockService.Setup(x => x.GetPlayers())
        //        .Returns(() => playerList);

        //    mockService.Setup(x => x.CreatePlayer(It.IsAny<string>()));

        //    var controller = new BattleshipController(_config, mockService.Object)
        //    {
        //        ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() { User = user } }
        //    };




        //    var result = controller.CreatePlayer();
        //    var apiResponse = (ApiResponse<string>)result;


        //    //Assert
        //    Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
        //    Assert.Equal("It seems game is in progress as 2 players have already signed in. Please try after some time.", apiResponse.ResultDto.Error);
        //}

        //[Fact]
        //public void CreatePlayer_Fails_For_SameName_Player()
        //{

        //    var user = MockClaimsPrincipal();

        //    var mockService = new Mock<IBattleshipGameService>();

        //    var playerList = new List<string> { "alice"};

        //    mockService.Setup(x => x.GetPlayers())
        //        .Returns(() => playerList);

        //    mockService.Setup(x => x.CreatePlayer(It.IsAny<string>()));

        //    var controller = new BattleshipController(_config, mockService.Object)
        //    {
        //        ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() { User = user } }
        //    };


        //    var result = controller.CreatePlayer();
        //    var apiResponse = (ApiResponse<string>)result;


        //    //Assert
        //    Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
        //    Assert.Equal("Player with this name is already registered for this game. Please try after some time.", apiResponse.ResultDto.Error);
        //}



        [Fact]
        public void CreateBoard_Returns_Success()
        {

            var user = MockClaimsPrincipal();

            var mockService = new Mock<IBattleshipGameService>();

           // var playerList = new List<string> { "alice" };

            mockService.Setup(x => x.CreateBoard(It.IsAny<string>(),It.IsAny<int>()))
                .Returns(() => true);

            var controller = new BattleshipController(_config, mockService.Object)
            {
                ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() { User = user } }
            };


            var result = controller.CreateBoard();
            var apiResponse = (ApiResponse<string>)result;


            //Assert
            Assert.Equal(HttpStatusCode.Created, apiResponse.StatusCode);
        }


        [Fact]
        public void CreateBoard_Fails_On_ReCreate()
        {

            var user = MockClaimsPrincipal();

            var mockService = new Mock<IBattleshipGameService>();

            // var playerList = new List<string> { "alice" };

            mockService.Setup(x => x.CreateBoard(It.IsAny<string>(), It.IsAny<int>()))
                .Throws(new BattleshipGameException("Board is already created"));

            var controller = new BattleshipController(_config, mockService.Object)
            {
                ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() { User = user } }
            };


            var result = controller.CreateBoard();
            var apiResponse = (ApiResponse<string>)result;


            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
        }



        [Fact]
        public void AddBattleShip_Returns_Success()
        {

            var user = MockClaimsPrincipal();

            var mockService = new Mock<IBattleshipGameService>();

            mockService.Setup(x => x.AddBattleship(It.IsAny<string>(), It.IsAny<Ship>(),
                    It.IsAny<Coordinates>(), It.IsAny<ShipPlacement>()))
                .Returns(true);

            var controller = new BattleshipController(_config, mockService.Object)
            {
                ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() { User = user } }
            };


            var result = controller.AddBattleShip(new BattleshipModel());
            var apiResponse = (ApiResponse<string>)result;


            //Assert
            Assert.Equal(HttpStatusCode.OK, apiResponse.StatusCode);
        }


        [Fact]
        public void AddBattleShip_Fails_BattleshipGameException()
        {

            var user = MockClaimsPrincipal();

            var mockService = new Mock<IBattleshipGameService>();

            // var playerList = new List<string> { "alice" };

            mockService.Setup(x => x.AddBattleship(It.IsAny<string>(), It.IsAny<Ship>(),
                    It.IsAny<Coordinates>(),It.IsAny<ShipPlacement>()))
                .Throws(new BattleshipGameException());

            var controller = new BattleshipController(_config, mockService.Object)
            {
                ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() { User = user } }
            };


            var result = controller.AddBattleShip(new BattleshipModel());
            var apiResponse = (ApiResponse<string>)result;


            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
        }



        [Fact]
        public void Attack_Returns_Success()
        {

            var user = MockClaimsPrincipal();

            var mockService = new Mock<IBattleshipGameService>();

            // var playerList = new List<string> { "alice" };

            mockService.Setup(x => x.Attack(It.IsAny<string>(), It.IsAny<Coordinates>()))
                .Returns(ShipState.Hit);

            var controller = new BattleshipController(_config, mockService.Object)
            {
                ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() { User = user } }
            };


            var result = controller.Attack(new AttackModel());
            var apiResponse = (ApiResponse<string>)result;


            //Assert
            Assert.Equal(HttpStatusCode.OK, apiResponse.StatusCode);
        }


        [Fact]
        public void Attack_Fails_BattleshipGameException()
        {

            var user = MockClaimsPrincipal();

            var mockService = new Mock<IBattleshipGameService>();

            // var playerList = new List<string> { "alice" };

            mockService.Setup(x => x.Attack(It.IsAny<string>(), It.IsAny<Coordinates>()))
                .Throws(new BattleshipGameException());

            var controller = new BattleshipController(_config, mockService.Object)
            {
                ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() { User = user } }
            };


            var result = controller.Attack(new AttackModel());
            var apiResponse = (ApiResponse<string>)result;


            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, apiResponse.StatusCode);
        }

        #region Helpers

        //private BattleshipController ControllerCreatePlayer()
        //{
        //    var user = MockClaimsPrincipal();

        //    var mockService = new Mock<IBattleshipGameService>();

        //    mockService.Setup(x => x.GetPlayers())
        //        .Returns(() => new List<string>());

        //    mockService.Setup(x => x.CreatePlayer(It.IsAny<string>()));

        //    var controller = new BattleshipController(_config, mockService.Object)
        //    {
        //        ControllerContext = new ControllerContext() { HttpContext = new DefaultHttpContext() { User = user } }
        //    };

        //    return controller;
        //}

        private static ClaimsPrincipal MockClaimsPrincipal()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("cognito:username", "alice"),
            }, "mock"));
            return user;
        }

        #endregion
    }
}
