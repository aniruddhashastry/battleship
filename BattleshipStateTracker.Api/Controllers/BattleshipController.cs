using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BattleshipStateTracker.Api.Helper;
using BattleshipStateTracker.Api.Models;
using BattleshipStateTracker.Service.CustomExceptions;
using BattleshipStateTracker.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace BattleshipStateTracker.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BattleshipController : ControllerBase
    {
        private readonly IOptions<BattleshipGameSettings> _config;
        private readonly IBattleshipGameService _battleshipGameService;

        public BattleshipController(IOptions<BattleshipGameSettings> config, IBattleshipGameService battleshipGameService)
        {
            _config = config;
            _battleshipGameService = battleshipGameService;
        }



        [HttpPost]
        [Route("createplayer")]
        public IActionResult CreatePlayer()
        {

            try
            {               
                var playerName = GetCurrentUserName();

                _battleshipGameService.CreatePlayer(playerName);

                return ResponseMessageHelper.OkCreated($"Good Luck! {playerName}");
            }
            catch (BattleshipGameException ex)
            {
                return ResponseMessageHelper.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                //write exception to a error log

                return ResponseMessageHelper.InternalServerError();
            }


        }


        [HttpPost]
        [Route("createboard")]
        public IActionResult CreateBoard()
        {
            try
            {
                string username = GetCurrentUserName();
                _battleshipGameService.CreateBoard(username, _config.Value.BoardSize);
                return ResponseMessageHelper.OkCreated("Board created");
            }
            catch (BattleshipGameException ex)
            {
                return ResponseMessageHelper.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                //write exception to a error log

                return ResponseMessageHelper.InternalServerError();
            }


        }

        [HttpPost]
        [Route("addbattleship")]
        public IActionResult AddBattleShip(BattleshipModel battleshipModel)
        {
            try
            {
                string username = GetCurrentUserName();
                var result = _battleshipGameService
                    .AddBattleship(username, battleshipModel.Ship
                    , battleshipModel.StartPosition, battleshipModel.ShipPlacement);

                if (result)
                {
                    return ResponseMessageHelper.Ok("Ship Added");

                }
                else
                {
                    return ResponseMessageHelper.InternalServerError();
                }
            }
            catch (BattleshipGameException ex)
            {
                return ResponseMessageHelper.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                //write exception to a error log

                return ResponseMessageHelper.InternalServerError();
            }


        }


        [HttpPost]
        [Route("attack")]
        public IActionResult Attack(AttackModel attackModel)
        {
            try
            {
                string username = GetCurrentUserName();

                var result = _battleshipGameService
                    .Attack(username, attackModel.Position);
                return ResponseMessageHelper.Ok(result.ToString());
            }
            catch (BattleshipGameException ex)
            {
                return ResponseMessageHelper.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                //write exception to a error log

                return ResponseMessageHelper.InternalServerError();
            }

        }



        [HttpPost]
        [Route("reset")]
        public IActionResult Reset()
        {
            try
            {
                _battleshipGameService.ResetGame();
                return ResponseMessageHelper.Ok("");
            }
            catch (BattleshipGameException ex)
            {
                return ResponseMessageHelper.BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                //write exception to a error log
                return ResponseMessageHelper.InternalServerError();
            }

        }


        #region Helper

        public string GetCurrentUserName()
        {
            return User.Claims.FirstOrDefault(c => c.Type == "cognito:username")?.Value;
        }


        #endregion
    }
}