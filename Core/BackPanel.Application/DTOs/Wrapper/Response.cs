namespace BackPanel.Application.DTOs.Wrapper;

public class Response<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; }
    public IList<string>? Errors { get; set; }
    public Response(T? data = default, bool success = true, string message = "", IList<string>? errors = null)
    {
        Data = data;
        Success = success;
        Message = message;
        Errors = errors;
    }
}