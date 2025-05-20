using System.Net;

namespace Domain.ApiResponse;

public class Response<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public T? Data { get; set; }
    public int StatusCode { get; set; }

    public Response(T? data, string message)
    {
        IsSuccess = true;
        Message = message;
        Data = data;
        StatusCode = (int)HttpStatusCode.OK;
    }
    public Response(string message, HttpStatusCode statusCode)
    {
        IsSuccess = false;
        Message = message;
        StatusCode = (int)statusCode;
        Data = default;
    }
}