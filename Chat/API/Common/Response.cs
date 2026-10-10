namespace API.Common
{
    public class Response<T>
    {
        public bool IsSuccess { get; }
        public T Data { get; }
        public string? Error { get; }
        public string? Message { get; }
        public Response(bool isSuccess, T data, string? error, string? meassage)
        {
            IsSuccess = isSuccess;
            Data = data;
            Error = error;
            Message = meassage;
        }

        public static Response<T> Success(T data, string message = "")
        {
            return new Response<T>(true, data, null, message);
        }
        public static Response<T> Failure(string error, string message = "")
        {
            return new Response<T>(false, default(T)!, error, message);
        }

    }
}
