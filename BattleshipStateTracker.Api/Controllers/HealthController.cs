using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipStateTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public bool Ping()
        {
            return true;
        }


        [HttpGet]
        [Route("ping")]
        public bool Ping1()
        {
            return true;
        }
    }
}