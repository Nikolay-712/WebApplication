namespace WebApp.Models;

public class ResponseContent<TResponse> : ResponseContent
{
    public TResponse Result { get; set; }
}


public class ResponseContent
{
    public ErrorResponse ErrorResponse { get; set; }
}