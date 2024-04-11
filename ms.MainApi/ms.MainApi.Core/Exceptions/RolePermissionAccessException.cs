using ms.MainApi.Core.Models;

namespace ms.MainApi.Core.Exceptions;

public class RolePermissionAccessException : Exception
{
    public ErrorResponse ErrorResponse { get; }

    public RolePermissionAccessException(ErrorResponse errorResponse) : base("One or more permission errors occured")
    {
        ErrorResponse = errorResponse ?? throw new ArgumentNullException(nameof(errorResponse));
    }

    public RolePermissionAccessException(List<ErrorModel> validationErrors) : base("One or more permission errors occured")
    {
        ErrorResponse = new ErrorResponse(validationErrors);
    }

    public RolePermissionAccessException(string message, params object[] values) : base(string.Format(message, values))
    {
        ErrorResponse = new ErrorResponse();
    }
}
