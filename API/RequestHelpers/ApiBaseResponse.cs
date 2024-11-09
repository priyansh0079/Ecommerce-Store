namespace API.RequestHelpers
{
    public class ApiBaseResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Content { get; set; }
    }
}