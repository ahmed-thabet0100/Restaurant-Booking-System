namespace Restaurant.Service.Implementation
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static Result<T> Success(T data, string message = null)
            => new Result<T> { IsSuccess = true, Data = data, Message = message };

        public static Result<T> Fail(string message)
            => new Result<T> { IsSuccess = false, Message = message };
    }

}
