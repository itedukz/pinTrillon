using ms.MainApi.Entity.Models.Dtos.Identities.PermissionServices;

namespace ms.MainApi.Entity.Models.Services;

public class PermissionResponse
{
    public bool isSuccess { get; set; } = false;
    public string message { get; set; } = "";
    //public ListDictionary? permissionDictionaries { get; set; }
    public PermissionList permittedActions { get; set; } = new();
    public bool canGetAll { get; set; } = false;
    public bool canGetOwn { get; set; } = false;

    public PermissionResponse()
    {
        isSuccess = false;
        message = "";
    }

    public PermissionResponse(bool _isSuccess)
    {
        isSuccess = _isSuccess;
        message = "";
    }

    public PermissionResponse(bool _isSuccess, string _message)
    {
        isSuccess = _isSuccess;
        message = _message;
    }
}
