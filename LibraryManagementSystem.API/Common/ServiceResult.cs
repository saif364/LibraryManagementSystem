namespace LibraryManagementSystem.API.Common
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }
    }
}
