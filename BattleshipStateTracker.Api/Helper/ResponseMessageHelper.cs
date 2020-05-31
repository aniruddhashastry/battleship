using System.Linq;
using BattleshipStateTracker.Api.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace BattleshipStateTracker.Api.Helper
{
    public static class ResponseMessageHelper
    {

        public static ApiResponse<string> InternalServerError(string error = "")
        {
            if (string.IsNullOrEmpty(error))
            {
                error = "Oops, something went wrong :(";
            }
            return new ApiResponse<string>(new ApiResultDto<string>(false, "", error), System.Net.HttpStatusCode.InternalServerError);
        }


        public static ApiResponse<string> BadRequest(string error = "")
        {
            if (string.IsNullOrEmpty(error))
            {
                error = "Oops, something went wrong :(";
            }
            return new ApiResponse<string>(new ApiResultDto<string>(false, "", error), System.Net.HttpStatusCode.BadRequest);
        }


        public static ApiResponse<string> Unauthorized(string error = "")
        {
            return new ApiResponse<string>(new ApiResultDto<string>(false, "", error), System.Net.HttpStatusCode.Unauthorized);
        }


        public static ApiResponse<string> Ok(string data)
        {
            return new ApiResponse<string>(new ApiResultDto<string>(true, data, ""), System.Net.HttpStatusCode.OK);
        }


        public static ApiResponse<string> OkNoContent()
        {
            return new ApiResponse<string>(new ApiResultDto<string>(true, "", ""), System.Net.HttpStatusCode.NoContent);
        }

        public static ApiResponse<string> OkCreated(string data)
        {
            return new ApiResponse<string>(new ApiResultDto<string>(true, data, ""), System.Net.HttpStatusCode.Created);
        }


        public static IActionResult ModelStateInvalid(ModelStateDictionary modelState)
        {
            var errorList = (from item in modelState
                where item.Value.Errors.Any()
                select item.Value.Errors[0].ErrorMessage).ToList();
            return ResponseMessageHelper.BadRequest(JsonConvert.SerializeObject(errorList));
        }

    }
}
