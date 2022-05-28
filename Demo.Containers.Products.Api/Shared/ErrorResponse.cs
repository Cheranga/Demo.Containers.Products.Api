namespace Demo.Containers.Products.Api.Shared;

public class ErrorResponse
{
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }

    public List<ErrorData> Errors { get; set; }

    public ErrorResponse()
    {
        Errors = new List<ErrorData>();
    }
}

public class ErrorData
{
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}