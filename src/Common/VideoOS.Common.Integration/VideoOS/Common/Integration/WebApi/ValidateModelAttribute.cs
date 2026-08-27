using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace VideoOS.Common.Integration.WebApi;

[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
public sealed class ValidateModelAttribute : ActionFilterAttribute
{
	public override void OnActionExecuting(HttpActionContext actionContext)
	{
		if (actionContext == null)
		{
			throw new ArgumentNullException("actionContext");
		}
		if (!actionContext.ModelState.IsValid)
		{
			actionContext.Response = HttpRequestMessageExtensions.CreateErrorResponse(actionContext.Request, HttpStatusCode.BadRequest, actionContext.ModelState);
		}
	}
}
