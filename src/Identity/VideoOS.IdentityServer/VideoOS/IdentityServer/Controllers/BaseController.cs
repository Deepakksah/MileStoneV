using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VideoOS.IdentityServer.Model;
using VideoOS.IdentityServer.Model.Responses;

namespace VideoOS.IdentityServer.Controllers;

public abstract class BaseController : ControllerBase
{
	[NonAction]
	public override BadRequestResult BadRequest()
	{
		throw new NotImplementedException("Use an overload to return details about the error to the client.");
	}

	[NonAction]
	public BadRequestObjectResult BadRequest([ActionResultObjectValue] IEnumerable<IdentityError> errors)
	{
		return new BadRequestObjectResult(new ErrorResponse(errors));
	}

	[NonAction]
	public override BadRequestObjectResult BadRequest([ActionResultObjectValue] object error)
	{
		CheckErrorResponseType(error);
		return base.BadRequest(error);
	}

	[NonAction]
	public BadRequestObjectResult BadRequest([ActionResultObjectValue] ApiError apiError)
	{
		return base.BadRequest(new ErrorResponse(apiError));
	}

	[NonAction]
	public override BadRequestObjectResult BadRequest([ActionResultObjectValue] ModelStateDictionary modelState)
	{
		ErrorResponse error = ErrorResponse.FromInvalidModelState(modelState);
		return base.BadRequest(error);
	}

	[NonAction]
	public NotFoundObjectResult NotFound([ActionResultObjectValue] ApiError apiError)
	{
		return base.NotFound(new ErrorResponse(apiError));
	}

	[NonAction]
	public NotFoundObjectResult UserNotFound(string id)
	{
		return base.NotFound(new ErrorResponse(new ApiCodes().UserNotFound(id)));
	}

	[NonAction]
	public NotFoundObjectResult RoleNotFound(string id)
	{
		return base.NotFound(new ErrorResponse(new ApiCodes().RoleNotFound(id)));
	}

	[NonAction]
	public override NotFoundObjectResult NotFound(object value)
	{
		CheckErrorResponseType(value);
		return base.NotFound(value);
	}

	[NonAction]
	public ConflictObjectResult Conflict([ActionResultObjectValue] ApiError apiError)
	{
		return base.Conflict(new ErrorResponse(apiError));
	}

	private void CheckErrorResponseType(object error)
	{
		if (!(error is ErrorResponse))
		{
			throw new NotImplementedException("Use an error that inherits from ErrorResponse");
		}
	}
}
