using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VideoOS.IdentityServer.Filters;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class CheckModelForNullAttribute : ActionFilterAttribute
{
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		string[] array = (from pair in context.ActionArguments
			where pair.Value == null
			select pair.Key).ToArray();
		if (array.Any())
		{
			context.Result = new BadRequestObjectResult(new
			{
				Details = "The arguments (" + string.Join(", ", array) + ") cannot be null"
			});
		}
	}
}
