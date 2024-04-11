using System.Net;

namespace ms.MainApi.Core.Results;

public interface IResult
{
    public HttpStatusCode StatusCode { get; set; }
    public bool Result { get; }
    public string Message { get; }
    public string? ItemId { get; set; }
    public List<string> ErrorMessages { get; set; }
}