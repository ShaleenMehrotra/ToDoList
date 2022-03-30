namespace Models
{
    public class ApiResponseBase
    {
        public Result Result { get; set; }
    }
    public class Result
    {
        public string Message { get; set; }
    }
}
