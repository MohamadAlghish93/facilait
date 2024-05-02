public class RequestResult
{
    
    public object Response { get; set; }

    public string Message { get; set; } = string.Empty;
    public bool IsSuccess { get; set; } = true;

    public string? Status { get; set; }
    
}