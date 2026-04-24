namespace AgroTrace.Aplication.DTO
{
    public class Response
    {
        public bool ThereIsError => Errors.Any();
        public long EntityId { get; set; }
        public bool Successful { get; set; }
        public string Message { get; set; } = string.Empty;

        public List<string> Errors { get; set; } = new List<string>(0);
    }


    public class Response<T> : Response
    {
        public T Data { get; set; }
        public IEnumerable<T> DataList { get; set; } = new List<T>();

        public  Response<T> Fail<T>(string message)
        {
            return new Response<T>
            {
                Successful = false,
                Message = message
            };
        }
    }


}


