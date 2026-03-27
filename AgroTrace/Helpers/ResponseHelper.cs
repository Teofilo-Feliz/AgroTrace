using AgroTrace.DTO;

namespace AgroTrace.Helpers
{
    public static class ResponseHelper
    {
        public static Response<T> Fail<T>(string message)
        {
            return new Response<T>
            {
                Successful = false,
                Message = message,
                Errors = new List<string> { message }
            };
        }

        public static Response<T> Ok<T>(T data, string message = "Success")
        {
            return new Response<T>
            {
                Successful = true,
                Message = message,
                Data = data
            };
        }
    }
}
