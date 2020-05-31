namespace BattleshipStateTracker.Api.Models.Response
{
    public class ApiResultDto<T>
    {
        public ApiResultDto(bool success, T data, string error)
        {
            Success = success;
            Data = data;
            Error = error;
        }

        public T Data { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }
}
