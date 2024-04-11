using Microsoft.AspNetCore.Http;
using ms.MainApi.Core.Exceptions;
using ms.MainApi.Core.Models;
using System.Text.Json;
using System.Net;

namespace ms.MainApi.Business.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _requestDelegate;

	public ExceptionMiddleware(RequestDelegate requestDelegate)
	{
		_requestDelegate = requestDelegate;
	}

	public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext context)
	{
		try
		{
			await _requestDelegate(context);
		}
		catch (Exception ex)
		{
			HandleExceptionAsync(context, ex);
		}
	}

	private static async Task HandleExceptionAsync(Microsoft.AspNetCore.Http.HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

		var errorResponse = new ErrorResponse();

		switch (exception)
		{
			case ValidationErrorsExcception validationErrorsExcception:
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
				errorResponse.Errors.AddRange(validationErrorsExcception.ErrorResponse.Errors); break;

			case OperationCanceledException: 
				context.Response.StatusCode = (int)HttpStatusCode.GatewayTimeout;
				errorResponse.AddError("Timeout", "Request timed out", "Operation canceled"); break;

			case RolePermissionAccessException rolePermissionAccessException:
				context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
				errorResponse.Errors.AddRange(rolePermissionAccessException.ErrorResponse.Errors); break;

			default: context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				errorResponse.AddError("Internal Error", "Internal Server Error has occured", exception.Message); break;
        }

		var json = JsonSerializer.Serialize(errorResponse);
		await context.Response.WriteAsync(json);
	}
}
