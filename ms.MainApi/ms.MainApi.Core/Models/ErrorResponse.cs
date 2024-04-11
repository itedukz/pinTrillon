namespace ms.MainApi.Core.Models;

public class ErrorResponse
{
	public List<ErrorModel> Errors { get; set; }

	public ErrorResponse()
	{
        Errors = new List<ErrorModel>();
    }

	public ErrorResponse(List<ErrorModel> errors)
	{
		Errors = errors;
	}

	public void AddError(ErrorModel error)
	{
		Errors.Add(error);
	}

	public void AddError(string scope, string exceptionName, string message)
	{
		var error = new ErrorModel(scope, exceptionName, message);
		Errors.Add(error);
	}

    public void AddErrors(List<ErrorModel> errors)
    {
        Errors.AddRange(errors);
    }
}
