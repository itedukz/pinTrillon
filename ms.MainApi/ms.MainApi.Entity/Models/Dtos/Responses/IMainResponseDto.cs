using ms.MainApi.Core.GeneralHelpers;
using ms.MainApi.Entity.Models.Dtos.Identities.PermissionServices;
using System.Net;

namespace ms.MainApi.Entity.Models.Dtos.Responses;

public interface IMainResponseDto
{
    public PaginationReturnModel? pagination { get; set; }
    public object? data { get; set; }
    public PermissionList permissions { get; set; }
    public bool isSuccess { get; set; }
    public string message { get; }
    public List<string>? errorMessages { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}

public class MainResponseDto : IMainResponseDto
{
    public PaginationReturnModel? pagination { get; set; } = null;
    public object? data { get; set; } = new object();
    public PermissionList permissions { get; set; } = new();
    public bool isSuccess { get; set; } = true;
    public string message { get; }
    public List<string>? errorMessages { get; set; } = null;
    public HttpStatusCode StatusCode { get; set; }

    public MainResponseDto(List<string> _errorMessages, HttpStatusCode _statusCode)
    {
        message = "";
        errorMessages = _errorMessages;
        StatusCode = _statusCode;
    }

    public MainResponseDto(string _message, List<string> _errorMessages, HttpStatusCode _statusCode)
    {
        message = _message;
        errorMessages = _errorMessages;
        StatusCode = _statusCode;
    }

    public MainResponseDto(string _message, HttpStatusCode _statusCode)
    {
        message = _message;
        StatusCode = _statusCode;
    }
    
    public MainResponseDto(string _message)
    {
        message = _message;
        StatusCode = HttpStatusCode.OK;
    }
    
    public MainResponseDto(object _data)
    {
        data = _data;
        message = "";
        StatusCode = HttpStatusCode.OK;
    }
    
    public MainResponseDto(object _data, string _message)
    {
        data = _data;
        message = _message;
        StatusCode = HttpStatusCode.OK;
    }

    public MainResponseDto(object _data, PermissionList permission)
    {
        data = _data;
        permissions = permission;
        message = "";
        StatusCode = HttpStatusCode.OK;
    }

    public MainResponseDto(object _data, PermissionList permission, PaginationReturnModel _pagination)
    {
        data = _data;
        permissions = permission;
        pagination = _pagination;
        message = "";
        StatusCode = HttpStatusCode.OK;
    }

    public MainResponseDto(object _data, PermissionList permission, int totalItems, int page, int pageSize)
    {
        data = _data;
        permissions = permission;
        pagination = GetPagination(totalItems, page, pageSize);
        message = "";
        StatusCode = HttpStatusCode.OK;
    }
    
    public MainResponseDto(object _data, int totalItems, int page, int pageSize)
    {
        data = _data;
        pagination = GetPagination(totalItems, page, pageSize);
        message = "";
        StatusCode = HttpStatusCode.OK;
    }


    private PaginationReturnModel GetPagination(int totalItems, int page, int pageSize)
    {
        pageSize = pageSize > 0 ? pageSize : 10;
        page = page > 0 ? page : 1;

        return new PaginationReturnModel
        {
            currentPage = page,
            pageSize = pageSize,
            totalItems = totalItems,
            totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize)
        };
    }
}
