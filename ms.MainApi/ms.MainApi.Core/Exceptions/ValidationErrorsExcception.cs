using ms.MainApi.Core.Models;

namespace ms.MainApi.Core.Exceptions;

public class ValidationErrorsExcception : Exception
{
    public ErrorResponse ErrorResponse { get; }

    public ValidationErrorsExcception(ErrorResponse errorResponse) : base("One or more validation errors occured")
    {
        ErrorResponse = errorResponse ?? throw new ArgumentNullException(nameof(errorResponse));
    }

    public ValidationErrorsExcception(List<ErrorModel> validationErrors) : base("One or more validation errors occured")
    {
        ErrorResponse = new ErrorResponse(validationErrors);
    }

    public ValidationErrorsExcception(string message, params object[] values) : base(string.Format(message, values))
    {
        ErrorResponse = new ErrorResponse();
    }
}
