namespace ms.MainApi.Core.Models;

public class ErrorModel
{
    public string Scope { get; set; }
    public string ExceptionName { get; set; }
    public string Message { get; set; }
    public string Detail { get; set; }


    public ErrorModel(string scope, string exceptionName, string message)
    {
        Scope = scope;
        ExceptionName = exceptionName;
        Message = message;
    }
    public ErrorModel(string scope, string exceptionName, string message, string detail)
    {
        Scope = scope;
        ExceptionName = exceptionName;
        Message = message;
        Detail = detail;
    }
    public ErrorModel() { }
}
