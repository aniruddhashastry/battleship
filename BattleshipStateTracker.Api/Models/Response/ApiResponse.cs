using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipStateTracker.Api.Models.Response
{
    public class ApiResponse<T> : IActionResult
    {

        public HttpStatusCode StatusCode { get; set; }

        public ApiResultDto<T> ResultDto;


        public ApiResponse(ApiResultDto<T> resultDto, HttpStatusCode statusCode) 
        {
            ResultDto = resultDto;
            StatusCode = statusCode;

        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(ResultDto)
            {
                StatusCode = (int)StatusCode
            };
            await objectResult.ExecuteResultAsync(context);
        }
    }
}
