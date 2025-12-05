namespace orac.api.DTOS
{
    public class ApiResponse<T>
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public T? Response { get; set; }

        public ApiResponse(T? data, string message = "OK", string status = "success")
        {
            Status = status;
            Message = message;
            Response = data;
        }

        public static ApiResponse<T> Success(T data, string message = "OK")
        {
            return new ApiResponse<T>(data, message, "success");
        }

        public static ApiResponse<T> Error(string message, T? data = default)
        {
            return new ApiResponse<T>(data, message, "error");
        }
    }
}
