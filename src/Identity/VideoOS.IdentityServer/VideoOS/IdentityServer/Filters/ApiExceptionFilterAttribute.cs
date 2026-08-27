using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using VideoOS.IdentityServer.Exceptions;
using VideoOS.IdentityServer.Model;
using VideoOS.IdentityServer.Model.Responses;

namespace VideoOS.IdentityServer.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
	public override void OnException(ExceptionContext context)
	{
		if (context.Exception is ApiException ex)
		{
			ObjectResult objectResult = ((ex.ErrorResponse == null) ? new ObjectResult(new ErrorResponse(new List<ApiError>
			{
				new ApiError
				{
					Description = ex.Message
				}
			})) : new ObjectResult(ex.ErrorResponse));
			objectResult.StatusCode = ex.StatusCode;
			context.Result = objectResult;
		}
	}
}
