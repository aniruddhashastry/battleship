using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using BattleshipStateTracker.Api.Helper;
using BattleshipStateTracker.Api.Models;
using BattleshipStateTracker.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;


namespace BattleshipStateTracker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly RegionEndpoint _region = RegionEndpoint.APSoutheast2;
        private readonly IOptions<BattleshipGameSettings> _config;
        private readonly IBattleshipGameService _battleshipGameService;


        public AuthenticationController(IOptions<BattleshipGameSettings> config
            , IBattleshipGameService battleshipGameService)
        {
            _config = config;
            _battleshipGameService = battleshipGameService;

        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn(User user)
        {

            if (!ModelState.IsValid)
            {
                return ResponseMessageHelper.ModelStateInvalid(ModelState);
            }

            var cognito = new AmazonCognitoIdentityProviderClient(_region);

            var request = new AdminInitiateAuthRequest
            {
                UserPoolId = _config.Value.AwsCognitoUserPoolId,
                ClientId = _config.Value.AwsCognitoAppClientId,
                AuthFlow = AuthFlowType.ADMIN_NO_SRP_AUTH
            };

            request.AuthParameters.Add("USERNAME", user.Username);
            request.AuthParameters.Add("PASSWORD", user.Password);

            try
            {
                var response = await cognito.AdminInitiateAuthAsync(request);

                return ResponseMessageHelper.Ok(response.AuthenticationResult.IdToken);

            }
            catch (NotAuthorizedException ex)
            {
                return ResponseMessageHelper.Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {

                return ResponseMessageHelper.InternalServerError(ex.Message);
            }

        }
    }
}